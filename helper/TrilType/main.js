const fs = require('fs');

const types = [
    'Object',
    'Int',
    'Bool',
];

let typeString = "";

for (let index = 0; index < types.length; index++) {
    typeString += `public static TrilType ${types[index]} = new ${types[index]}();\n`;
}

fs.writeFileSync('../../src/TrilType/TrilType.cs', `
namespace trilc
{
    class TrilType
    {
        ${typeString}
        public TrilType parent = null;
    }
}`);