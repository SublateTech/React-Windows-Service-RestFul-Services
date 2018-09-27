const { echo, exec } = require('shelljs');

const packages = [
    './node_modules/admin-on-rest',
];

echo('\nPre build starts.\n');
packages.forEach(pack => exec(`babel --presets=env ${pack} --out-dir ${pack}`));
echo('\nPre build finished.\n');