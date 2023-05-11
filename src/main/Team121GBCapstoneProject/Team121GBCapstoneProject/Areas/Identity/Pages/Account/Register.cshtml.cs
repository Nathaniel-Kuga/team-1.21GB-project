// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.Services;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Services.Abstract;

namespace Team121GBCapstoneProject.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IReCaptchaService _reCaptchaService;
        private readonly IPersonRepository _personRepository;
        private readonly IPersonListRepository _personListRepository;
        private readonly IListKindRepository _listKindRepository;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IReCaptchaService captchaService,
            IPersonRepository personRepository,
            IPersonListRepository personListRepository,
            IListKindRepository listKindRepository)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _reCaptchaService = captchaService;
            _personRepository = personRepository;
            _personListRepository = personListRepository;
            _listKindRepository = listKindRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        //var user = CreateUser();


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //
                //check the recaptcha
                if (!Request.Form.ContainsKey("g-recaptcha-response")) return Page();
                var captcha = Request.Form["g-recaptcha-response"].ToString();
                if (!await _reCaptchaService.IsValid(captcha)) return Page();

                // Added code to add First Name and Last Name
                var user = CreateUser();
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // * add person to project db
                    _personRepository.AddPersonToProjectDb(user.Id);
                    // * give new person their lists
                    Person person = _personRepository.GetAll()
                                                     .FirstOrDefault(person => person.AuthorizationId == user.Id);
                    List<ListKind> listKinds = _listKindRepository.GetAll()
                                                                  .Where(l => l.Id < 4)
                                                                  .ToList();// check that this is only the default lists, not custom
                    _personListRepository.AddDefaultListsOnAccountCreation(person, listKinds);
                    // * give person their Dalle credits
                    person.DallECredits = 5;
                    person = _personRepository.AddOrUpdate(person);

                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email, 
                        $"Welcome to the Ultimate Gmaing Hub {Input.FirstName}!: Please confirm your email",
                        $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n<html data-editor-version=\"2\" class=\"sg-campaigns\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n    <head>\r\n      <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n      <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1\">\r\n      <!--[if !mso]><!-->\r\n      <meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\">\r\n      <!--<![endif]-->\r\n      <!--[if (gte mso 9)|(IE)]>\r\n      <xml>\r\n        <o:OfficeDocumentSettings>\r\n          <o:AllowPNG/>\r\n          <o:PixelsPerInch>96</o:PixelsPerInch>\r\n        </o:OfficeDocumentSettings>\r\n      </xml>\r\n      <![endif]-->\r\n      <!--[if (gte mso 9)|(IE)]>\r\n  <style type=\"text/css\">\r\n    body {{width: 600px;margin: 0 auto;}}\r\n    table {{border-collapse: collapse;}}\r\n    table, td {{mso-table-lspace: 0pt;mso-table-rspace: 0pt;}}\r\n    img {{-ms-interpolation-mode: bicubic;}}\r\n  </style>\r\n<![endif]-->\r\n      <style type=\"text/css\">\r\n    body, p, div {{\r\n      font-family: inherit;\r\n      font-size: 14px;\r\n    }}\r\n    body {{\r\n      color: #000000;\r\n    }}\r\n    body a {{\r\n      color: #1188E6;\r\n      text-decoration: none;\r\n    }}\r\n    p {{ margin: 0; padding: 0; }}\r\n    table.wrapper {{\r\n      width:100% !important;\r\n      table-layout: fixed;\r\n      -webkit-font-smoothing: antialiased;\r\n      -webkit-text-size-adjust: 100%;\r\n      -moz-text-size-adjust: 100%;\r\n      -ms-text-size-adjust: 100%;\r\n    }}\r\n    img.max-width {{\r\n      max-width: 100% !important;\r\n    }}\r\n    .column.of-2 {{\r\n      width: 50%;\r\n    }}\r\n    .column.of-3 {{\r\n      width: 33.333%;\r\n    }}\r\n    .column.of-4 {{\r\n      width: 25%;\r\n    }}\r\n    ul ul ul ul  {{\r\n      list-style-type: disc !important;\r\n    }}\r\n    ol ol {{\r\n      list-style-type: lower-roman !important;\r\n    }}\r\n    ol ol ol {{\r\n      list-style-type: lower-latin !important;\r\n    }}\r\n    ol ol ol ol {{\r\n      list-style-type: decimal !important;\r\n    }}\r\n    @media screen and (max-width:480px) {{\r\n      .preheader .rightColumnContent,\r\n      .footer .rightColumnContent {{\r\n        text-align: left !important;\r\n      }}\r\n      .preheader .rightColumnContent div,\r\n      .preheader .rightColumnContent span,\r\n      .footer .rightColumnContent div,\r\n      .footer .rightColumnContent span {{\r\n        text-align: left !important;\r\n      }}\r\n      .preheader .rightColumnContent,\r\n      .preheader .leftColumnContent {{\r\n        font-size: 80% !important;\r\n        padding: 5px 0;\r\n      }}\r\n      table.wrapper-mobile {{\r\n        width: 100% !important;\r\n        table-layout: fixed;\r\n      }}\r\n      img.max-width {{\r\n        height: auto !important;\r\n        max-width: 100% !important;\r\n      }}\r\n      a.bulletproof-button {{\r\n        display: block !important;\r\n        width: auto !important;\r\n        font-size: 80%;\r\n        padding-left: 0 !important;\r\n        padding-right: 0 !important;\r\n      }}\r\n      .columns {{\r\n        width: 100% !important;\r\n      }}\r\n      .column {{\r\n        display: block !important;\r\n        width: 100% !important;\r\n        padding-left: 0 !important;\r\n        padding-right: 0 !important;\r\n        margin-left: 0 !important;\r\n        margin-right: 0 !important;\r\n      }}\r\n      .social-icon-column {{\r\n        display: inline-block !important;\r\n      }}\r\n    }}\r\n  </style>\r\n    <style>\r\n      @media screen and (max-width:480px) {{\r\n        table\\0 {{\r\n          width: 480px !important;\r\n          }}\r\n      }}\r\n    </style>\r\n      <!--user entered Head Start--><link href=\"https://fonts.googleapis.com/css?family=Fira+Sans+Condensed&display=swap\" rel=\"stylesheet\"><style>\r\n    body {{font-family: 'Fira Sans Condensed', sans-serif;}}\r\n</style><!--End Head user entered-->\r\n    </head>\r\n    <body>\r\n      <center class=\"wrapper\" data-link-color=\"#1188E6\" data-body-style=\"font-size:14px; font-family:inherit; color:#000000; background-color:#f0f0f0;\">\r\n        <div class=\"webkit\">\r\n          <table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\" class=\"wrapper\" bgcolor=\"#f0f0f0\">\r\n            <tr>\r\n              <td valign=\"top\" bgcolor=\"#f0f0f0\" width=\"100%\">\r\n                <table width=\"100%\" role=\"content-container\" class=\"outer\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n                  <tr>\r\n                    <td width=\"100%\">\r\n                      <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n                        <tr>\r\n                          <td>\r\n                            <!--[if mso]>\r\n    <center>\r\n    <table><tr><td width=\"600\">\r\n  <![endif]-->\r\n                                    <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:100%; max-width:600px;\" align=\"center\">\r\n                                      <tr>\r\n                                        <td role=\"modules-container\" style=\"padding:0px 0px 0px 0px; color:#000000; text-align:left;\" bgcolor=\"#FFFFFF\" width=\"100%\" align=\"left\"><table class=\"module preheader preheader-hide\" role=\"module\" data-type=\"preheader\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"display: none !important; mso-hide: all; visibility: hidden; opacity: 0; color: transparent; height: 0; width: 0;\">\r\n    <tr>\r\n      <td role=\"module-content\">\r\n        <p></p>\r\n      </td>\r\n    </tr>\r\n  </table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" role=\"module\" data-type=\"columns\" style=\"padding:30px 5px 30px 5px;\" bgcolor=\"#4d5171\" data-distribution=\"1,1\">\r\n    <tbody>\r\n      <tr role=\"module-content\">\r\n        <td height=\"100%\" valign=\"top\"><table width=\"285\" style=\"width:285px; border-spacing:0; border-collapse:collapse; margin:0px 10px 0px 0px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-0\">\r\n      <tbody>\r\n        <tr>\r\n          <td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"wrapper\" role=\"module\" data-type=\"image\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"d6873067-aad5-44d9-b8e3-8ee926481dac\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"font-size:6px; line-height:10px; padding:0px 0px 0px 0px;\" valign=\"top\" align=\"center\">\r\n          <img class=\"max-width\" border=\"0\" style=\"display:block; color:#000000; text-decoration:none; font-family:Helvetica, arial, sans-serif; font-size:16px; max-width:100% !important; width:100%; height:auto !important;\" width=\"285\" alt=\"\" data-proportionally-constrained=\"true\" data-responsive=\"true\" src=\"http://cdn.mcauto-images-production.sendgrid.net/75cebb6fd72171df/8558f8a4-a1ab-4f48-98f4-f77454927c03/922x328.png\">\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table></td>\r\n        </tr>\r\n      </tbody>\r\n    </table><table width=\"285\" style=\"width:285px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 10px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-1\">\r\n      <tbody>\r\n        <tr>\r\n          <td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"1a193ccc-4211-442e-a585-cd69fcff3a9c\" data-mc-module-version=\"2019-10-22\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:18px 0px 18px 0px; line-height:60px; text-align:inherit;\" height=\"100%\" valign=\"top\" bgcolor=\"\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: inherit\"><span style=\"color: #fafafa; font-size: 18px\"><strong>Ultimate Gaming Hub</strong></span></div><div></div></div></td>\r\n      </tr>\r\n    </tbody>\r\n  </table></td>\r\n        </tr>\r\n      </tbody>\r\n    </table></td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"080768f5-7b16-4756-a254-88cfe5138113\" data-mc-module-version=\"2019-10-22\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:30px 30px 18px 30px; line-height:36px; text-align:inherit; background-color:#4d5171;\" height=\"100%\" valign=\"top\" bgcolor=\"#4d5171\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: center\"><span style=\"color: #ffffff; font-size: 48px; font-family: inherit\">Welcome, {Input.FirstName}!</span></div><div></div></div></td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"cddc0490-36ba-4b27-8682-87881dfbcc14\" data-mc-module-version=\"2019-10-22\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:18px 30px 18px 30px; line-height:22px; text-align:inherit; background-color:#4d5171;\" height=\"100%\" valign=\"top\" bgcolor=\"#4d5171\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: inherit\"><span style=\"color: #ffffff; font-size: 15px\">Thanks so much for joining The Ultimate Gaming Hub! We are so excited to see you on our site. Here you can organize all your games to your hearts content as well as find new games that may interest you. To finish setting up your account please click the button below.</span></div><div></div></div></td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"module\" data-role=\"module-button\" data-type=\"button\" role=\"module\" style=\"table-layout:fixed;\" width=\"100%\" data-muid=\"cd669415-360a-41a6-b4b4-ca9e149980b3\">\r\n      <tbody>\r\n        <tr>\r\n          <td align=\"center\" bgcolor=\"#4d5171\" class=\"outer-td\" style=\"padding:10px 0px 40px 0px; background-color:#4d5171;\">\r\n            <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"wrapper-mobile\" style=\"text-align:center;\">\r\n              <tbody>\r\n                <tr>\r\n                <td align=\"center\" bgcolor=\"#ffc94c\" class=\"inner-td\" style=\"border-radius:6px; font-size:16px; text-align:center; background-color:inherit;\">\r\n                  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style=\"background-color:#ffc94c; border:1px solid #ffc94c; border-color:#ffc94c; border-radius:40px; border-width:1px; color:#3f4259; display:inline-block; font-size:15px; font-weight:normal; letter-spacing:0px; line-height:25px; padding:12px 18px 10px 18px; text-align:center; text-decoration:none; border-style:solid; font-family:inherit; width:168px;\" target=\"_blank\">Confirm Email</a>\r\n                </td>\r\n                </tr>\r\n              </tbody>\r\n            </table>\r\n          </td>\r\n        </tr>\r\n      </tbody>\r\n    </table><table class=\"module\" role=\"module\" data-type=\"divider\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"c5a3c312-4d9d-44ff-9fce-6a8003caeeca\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:0px 20px 0px 20px;\" role=\"module-content\" height=\"100%\" valign=\"top\" bgcolor=\"#4d5171\">\r\n          <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" height=\"1px\" style=\"line-height:1px; font-size:1px;\">\r\n            <tbody>\r\n              <tr>\r\n                <td style=\"padding:0px 0px 1px 0px;\" bgcolor=\"#ffc94c\"></td>\r\n              </tr>\r\n            </tbody>\r\n          </table>\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"eb301547-da19-441f-80a1-81e1b56e64ad\" data-mc-module-version=\"2019-10-22\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:30px 0px 18px 0px; line-height:22px; text-align:inherit; background-color:#4d5171;\" height=\"100%\" valign=\"top\" bgcolor=\"#4d5171\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: center\"><span style=\"color: #ffc94c; font-size: 20px; font-family: inherit\"><strong>Features</strong></span></div><div></div></div></td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table class=\"module\" role=\"module\" data-type=\"spacer\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"2931446b-8b48-42bd-a70c-bffcfe784680\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:0px 0px 10px 0px;\" role=\"module-content\" bgcolor=\"#4d5171\">\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" role=\"module\" data-type=\"columns\" style=\"padding:0px 20px 0px 20px;\" bgcolor=\"#4d5171\" data-distribution=\"1,1\">\r\n    <tbody>\r\n      <tr role=\"module-content\">\r\n        <td height=\"100%\" valign=\"top\"><table width=\"270\" style=\"width:270px; border-spacing:0; border-collapse:collapse; margin:0px 10px 0px 0px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-0\">\r\n      <tbody>\r\n        <tr>\r\n          <td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"module\" role=\"module\" data-type=\"spacer\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"a45551e7-98d7-40da-889d-a0dc41550c4e.1\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:0px 0px 15px 0px;\" role=\"module-content\" bgcolor=\"\">\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table class=\"wrapper\" role=\"module\" data-type=\"image\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"8ZPkEyRmw35sXLUWrdumXA\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"font-size:6px; line-height:10px; padding:0px 0px 0px 0px;\" valign=\"top\" align=\"center\">\r\n          <img class=\"max-width\" border=\"0\" style=\"display:block; color:#000000; text-decoration:none; font-family:Helvetica, arial, sans-serif; font-size:16px; max-width:75% !important; width:75%; height:auto !important;\" width=\"203\" alt=\"\" data-proportionally-constrained=\"true\" data-responsive=\"true\" src=\"http://cdn.mcauto-images-production.sendgrid.net/75cebb6fd72171df/499940ea-6bfa-49d9-98f7-e303a5eaf36d/600x300.png\">\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table></td>\r\n        </tr>\r\n      </tbody>\r\n    </table><table width=\"270\" style=\"width:270px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 10px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-1\">\r\n      <tbody>\r\n        <tr>\r\n          <td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"4vL54iw2MCdgWcxxaCgLhi\" data-mc-module-version=\"2019-10-22\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:18px 0px 18px 0px; line-height:20px; text-align:inherit;\" height=\"100%\" valign=\"top\" bgcolor=\"\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: inherit\"><span style=\"color: #9cfed4; font-size: 15px\"><strong>Searching for Games</strong></span></div>\r\n<div style=\"font-family: inherit; text-align: inherit\"><br></div>\r\n<div style=\"font-family: inherit; text-align: inherit\"><span style=\"color: #ffffff; font-size: 15px\">Search a huge catalog of games and add them to your list to organize you collection.</span></div><div></div></div></td>\r\n      </tr>\r\n    </tbody>\r\n  </table></td>\r\n        </tr>\r\n      </tbody>\r\n    </table></td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table class=\"module\" role=\"module\" data-type=\"spacer\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"2931446b-8b48-42bd-a70c-bffcfe784680.1\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:0px 0px 20px 0px;\" role=\"module-content\" bgcolor=\"#4d5171\">\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" role=\"module\" data-type=\"columns\" style=\"padding:0px 20px 30px 20px;\" bgcolor=\"#4d5171\" data-distribution=\"1,1\">\r\n    <tbody>\r\n      <tr role=\"module-content\">\r\n        <td height=\"100%\" valign=\"top\"><table width=\"265\" style=\"width:265px; border-spacing:0; border-collapse:collapse; margin:0px 15px 0px 0px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-0\">\r\n      <tbody>\r\n        <tr>\r\n          <td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"module\" role=\"module\" data-type=\"spacer\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"a45551e7-98d7-40da-889d-a0dc41550c4e\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:0px 0px 15px 0px;\" role=\"module-content\" bgcolor=\"\">\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table class=\"wrapper\" role=\"module\" data-type=\"image\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"8ZPkEyRmw35sXLUWrdumXA.1\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"font-size:6px; line-height:10px; padding:0px 0px 0px 0px;\" valign=\"top\" align=\"center\">\r\n          <img class=\"max-width\" border=\"0\" style=\"display:block; color:#000000; text-decoration:none; font-family:Helvetica, arial, sans-serif; font-size:16px; max-width:100% !important; width:100%; height:auto !important;\" width=\"265\" alt=\"\" data-proportionally-constrained=\"true\" data-responsive=\"true\" src=\"http://cdn.mcauto-images-production.sendgrid.net/75cebb6fd72171df/9cc53f52-f28c-4362-b466-365aa5ec7db2/2172x695.jpg\">\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table></td>\r\n        </tr>\r\n      </tbody>\r\n    </table><table width=\"265\" style=\"width:265px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 15px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-1\">\r\n      <tbody>\r\n        <tr>\r\n          <td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"4vL54iw2MCdgWcxxaCgLhi.1\" data-mc-module-version=\"2019-10-22\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:18px 0px 18px 0px; line-height:20px; text-align:inherit;\" height=\"100%\" valign=\"top\" bgcolor=\"\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: inherit\"><span style=\"color: #9cfed4; font-size: 15px\"><strong>Link your steam account</strong></span></div>\r\n<div style=\"font-family: inherit; text-align: inherit\"><br></div>\r\n<div style=\"font-family: inherit; text-align: inherit\"><span style=\"color: #ffffff; font-size: 15px\">Link your steam account with our site and organize your steam collection all in one site.</span></div><div></div></div></td>\r\n      </tr>\r\n    </tbody>\r\n  </table></td>\r\n        </tr>\r\n      </tbody>\r\n    </table></td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table class=\"module\" role=\"module\" data-type=\"divider\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"c5a3c312-4d9d-44ff-9fce-6a8003caeeca.1\">\r\n    <tbody>\r\n      <tr>\r\n        <td style=\"padding:0px 20px 0px 20px;\" role=\"module-content\" height=\"100%\" valign=\"top\" bgcolor=\"#4d5171\">\r\n          <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" height=\"1px\" style=\"line-height:1px; font-size:1px;\">\r\n            <tbody>\r\n              <tr>\r\n                <td style=\"padding:0px 0px 1px 0px;\" bgcolor=\"#ffc94c\"></td>\r\n              </tr>\r\n            </tbody>\r\n          </table>\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"module\" data-role=\"module-button\" data-type=\"button\" role=\"module\" style=\"table-layout:fixed;\" width=\"100%\" data-muid=\"5f89d789-2bfd-48e2-a219-1de42476c398\">\r\n      <tbody>\r\n        <tr>\r\n          <td align=\"center\" bgcolor=\"\" class=\"outer-td\" style=\"padding:0px 0px 20px 0px;\">\r\n            <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"wrapper-mobile\" style=\"text-align:center;\">\r\n              <tbody>\r\n                <tr>\r\n                <td align=\"center\" bgcolor=\"#f5f8fd\" class=\"inner-td\" style=\"border-radius:6px; font-size:16px; text-align:center; background-color:inherit;\"><a href=\"https://www.sendgrid.com/?utm_source=powered-by&utm_medium=email\" style=\"background-color:#f5f8fd; border:1px solid #f5f8fd; border-color:#f5f8fd; border-radius:25px; border-width:1px; color:#a8b9d5; display:inline-block; font-size:10px; font-weight:normal; letter-spacing:0px; line-height:normal; padding:5px 18px 5px 18px; text-align:center; text-decoration:none; border-style:solid; font-family:helvetica,sans-serif;\" target=\"_blank\">♥ POWERED BY TWILIO SENDGRID</a></td>\r\n                </tr>\r\n              </tbody>\r\n            </table>\r\n          </td>\r\n        </tr>\r\n      </tbody>\r\n    </table></td>\r\n                                      </tr>\r\n                                    </table>\r\n                                    <!--[if mso]>\r\n                                  </td>\r\n                                </tr>\r\n                              </table>\r\n                            </center>\r\n                            <![endif]-->\r\n                          </td>\r\n                        </tr>\r\n                      </table>\r\n                    </td>\r\n                  </tr>\r\n                </table>\r\n              </td>\r\n            </tr>\r\n          </table>\r\n        </div>\r\n      </center>\r\n    </body>\r\n  </html>");

                    /*                        $"<h1 style=\"text-align: inherit; font-family: inherit\"><span style=\"font-family: &quot;arial black&quot;, helvetica, sans-serif; font-size: 40px; color: #d89816\">Welcome to the Gaming Platform!</span></h1> Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style=\"background-color:#ffffff; border:1px solid #939598; border-color:#939598; border-radius:0px; border-width:1px; color:#D89816; display:inline-block; font-size:15px; font-weight:normal; letter-spacing:1px; line-height:normal; padding:16px 20px 16px 20px; text-align:center; text-decoration:none; border-style:solid; font-family:times new roman,times,serif;\">clicking here</a>.");
                    */
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
