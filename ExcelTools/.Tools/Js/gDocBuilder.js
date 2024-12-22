#!/usr/bin/env node
var fs = require('fs');
var path = require('path');
var program = require('commander');
var Builder = require('./BuilderUtils');
var getTable = require('./getTable');

program
    .version('0.0.1')
    .usage('<spreadsheet-id> <client-script-dir> <client-data-dir> <server-script-dir> <server-data-dir> [options]')
    .option('-u, --user [user]', 'Google account to login')
    .option('-p, --password [password]', 'Password to login')
    .option('-t, --token [token]', 'Auth token acquired externally')
    .option('-y, --tokentype [tokentype]', 'Type of the informed token (defaults to Bearer)')
    .option('-c, --hash [column]', 'Column to hash the final JSON')
    .option('-o, --oauth2 [oauth2]', 'Use OAuth2, defaults config is oauth2.json')
    .option('-l, --list-only', 'Ignore headers and just list the values in arrays')
    .option('-j, --json [json]', 'Output json directory')
    .option('-b, --beautify', 'Json use beautify format')
    .option('-f, --filename [json]', 'Json filename')
    .option('-s, --sheet', 'Output sheet')
    .parse(process.argv);

if (program.args.length < 5) {
    program.help();
}

var spreadsheetId = program.args[0];
var outputConfig = new Builder.OutputConfig(program.args[1], program.args[2], program.args[3], program.args[4]);

// get param 
var param = {  
    spreadsheetId: spreadsheetId,  
    worksheetName: null,
    typeRow: 1,
    dataRow: 3,
    isBySheet: 0
};

if (program.sheet) {
    param.isBySheet = 1;
}

getTable.getTable(param, function(result) {
    if (result == null || result == undefined) {
        return;
    }
    
    try {
        // console.error(param.spreadsheetId + " - " + result);
        resultContent = JSON.parse(result);
    }
    catch (e) {
        console.error(param.spreadsheetId + " return data is not json, export error, please check");
        return;
    }
    if (resultContent == null || resultContent == undefined) {
        console.error(param.spreadsheetId + " export error, please check");
        return;
    }
    if (resultContent['code'] != 0) {
        console.error(param.spreadsheetId + " export error, please check");
        console.log("error msg is : " + resultContent['msg']);
        return;
    }
    content = resultContent['data']
    
    var writeFile = function (filename, content) {
		// console.log('Output json begin: ', filename);
		fs.writeFile(filename.toLowerCase(), content, "utf-8", function (err) {
			if (err) {
				throw err;
			}
			//process.exit(0);
			console.log('Output:', filename);
		});
	};

    if (!program.sheet) {
        // put all sheets in one json file
		mkDir(program.json);
		var filename = joinDir(program.json, program.filename + ".json");
        var result = JSON.parse(content);
        var json = JSON.stringify(result, null, "    ");
		writeFile(filename, json);

	} else {
        var result = JSON.parse(content);
        for (var subjson in result) {
            var filename = joinDir(program.json, subjson + '.json');
            var json = JSON.stringify(result[subjson], null, "    ");
            writeFile(filename, json);
        }
	}
});

function strMapToObj(strMap) {
    let obj = Object.create(null);
    for (let [k,v] of strMap) {
        // We don’t escape the key '__proto__'
        // which can cause problems on older engines
        obj[k] = v;
    }
    return obj;
}
