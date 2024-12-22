const https = require('https');  
const qs = require('querystring');  

const config = {
    "host" : 'table-tools.dragonplus.com',
    //"host" : 'table-tools.ustargames.com',
    "port" : 443,
    "loadTablepath" : "/table/load",
    "loadTableMethod" : 'GET'
}

const options = {  
    hostname: config['host'],  
    port: config['port'],  
    path: config['loadTablepath'] + '?' ,  
    method: config['loadTableMethod'] 
};  

function getTable (param, callback) {
    if (!param.spreadsheetId) {
        console.log("spreadSheetId can't be null or empty");
        return;
    }
    options.path = options.path + qs.stringify(param);

    let req = https.request(options, function (res) {  
        res.setEncoding('utf8');  
        let result = '';
        res.on('data', function (chunk) {  
            result += chunk;
        }); 
        res.on('end', function() {
            callback(result);
        })
        
    });  

    req.end();
};

function getTableRaw(param, callback) {
    if (!param.spreadsheetId) {
        console.log("spreadSheetId can't be null or empty");
        return;
    }

    const params = JSON.parse(JSON.stringify(param));
    params.password = 'dragonplus';
    params.raw = 1;

    const opts = JSON.parse(JSON.stringify(options));
    opts.path = '/tools/googlespreadsheet/loadTables' + qs.stringify(params);

    let req = https.request(options, function (res) {  
        res.setEncoding('utf8');  
        let result = '';
        res.on('data', function (chunk) {  
            result += chunk;
        }); 
        res.on('end', function() {
            callback(result);
        })
        
    });  

    req.end();
};

module.exports = {
    getTable,
    getTableRaw,
};
