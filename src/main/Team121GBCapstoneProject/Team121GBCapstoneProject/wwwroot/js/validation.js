/*
 * Simple object validation.  Can check that objects have properties and also their values.
 * There are lots of JS libraries that will do this and much more, but plain JS works to get started.
 * */

// Expects a schema like this of what properties the object has
const schemaExample = {
    magnitude: x => typeof x === 'number' && x >= 0,                // check type and value
    location: x => typeof x === 'string' && /[A-Za-z0-9]*/.test(x), // can test by regex
    eTime: x => typeof x === 'number'
};

// Thanks to Patrick Roberts answer: https://stackoverflow.com/questions/38616612/javascript-elegant-way-to-check-object-has-required-properties
// Pass in the object to validate properties on, and the schema that defines what properties should exist and a validation function to run.
// Returns an array that is empty(passes validation), or contains Error object corresponding to property names that failed validation
const validateObject = (obj, schema) => Object
    .keys(schema)
    .filter(key => !schema[key](obj[key]))
    .map(key => new Error(`${key}`));

// Apply object validation to every object in this array.  Returns count of errors.  Could easily extend to returning where the errors are.
// Pass in an array of objects plus a schema,
// Returns the number of validation errors found
const validateArrayOfObjects = (arr, schema) => arr
    .map(obj => validateObject(obj, schema))
    .reduce((acc, arr) => arr.length > 0 ? acc + 1 : 0, 0);  // use a fold to count up the number of entries whose length is > 0

//* Client side validation of GameDto class
 
function validateGameDtoData(data) {
    if (data === undefined) return false;
    if (Object.keys(data).length === 0) return false;
    console.log("igdbID passed into validator ", data.igdbID);
    const schema = {
        listKind: x => typeof x === 'string' && x !== "",
        gameTitle: x => typeof x === 'string' && x !== "",
        imageSrc: x => typeof x === 'string' && x !== "",
        igdbID: x => typeof x === 'number'  && x > 0 // ? This should be false and yet returns true, or is being skipped
    };
    const errors = validateObject(data, schema);
    return errors.length === 0; // return true if there are no errors
}

function validateUserLists(data) {
    if (data === undefined) return false;
    const schema = {
        listKind: x => typeof x === 'string' && x !== ""
    }
    const errors = validateObject(data, schema);
    return errors.length === 0; // return true if there are no errors
}

export { validateObject, validateArrayOfObjects, validateGameDtoData, validateUserLists }