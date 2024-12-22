#!/usr/bin/env node
var fs = require('fs');
var path = require('path');
var program = require('commander');
var Builder = require('./BuilderUtilsStorage');
var getTable = require('./getTable');

program
    .version('0.0.1')
    .usage('<spreadsheet-id> <unity-script-dir> <unity-script-template> [options]')
    .parse(process.argv);

if (program.args.length < 3) {
    program.help();
}

var spreadsheetId = program.args[0];
var outputConfig = new Builder.OutputConfig(program.args[1], program.args[2]);

// get param 
var param = {  
    spreadsheetId: spreadsheetId,  
    worksheetName: null,
    typeRow: 1,
    dataRow: 3,
    isBySheet: 0
};

getTable.getTableRaw(param, function(result) {
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
        console.error(param.spreadsheetId + ", code(" + resultContent['code'] + ") export error, please check");
        console.log("error msg is : " + resultContent['msg']);
        return;
    }
    content = resultContent['data']

    Builder.Cleanup();

    var i = 0;
    for (var sheetName in content) {
        readsheet(sheetName, content[sheetName], ++i);
    }

    Builder.buildSpreadsheet(gssClassList, outputConfig);
    running = false;
    console.log('google spreadsheet finished!');
});

//queue to build
var gssClassList = [];
var running = false;

function readsheet(sheetName, sheetData, sheetIndex) {
    console.log('google spreadsheet: read the ', sheetIndex, ' ', 'sheet: ', sheetName);

    var jsTemplateMap = {};

    var rowProp = program.vertical ? "col" : "row";
    var colProp = program.vertical ? "row" : "col";

    var rows = [];
    var datas = sheetData;
    for (rowIndex in datas) {
        if (typeof rows[rowIndex] === "undefined") {
            rows[rowIndex] = [];
        }
        var rowdata = datas[rowIndex];
        // console.log('rowdata:', rowdata);
        for (colIndex in rowdata) {
            rows[rowIndex].push({row:rowIndex,col:colIndex,value:rowdata[colIndex]});
        };
    };

    rows.forEach(function(col) {
        col.sort(function(cell1, cell2) {
            return cell1[colProp] - cell2[colProp];
        });
    });

    var finalList = [];

    var properties = [];
    for (k in rows[0]) {
        var val = rows[0][k].value;
        // properties.push(val.charAt(0).toUpperCase() + val.slice(1));
        properties.push(val);
    }
    // console.log('properties:', properties);

    var valueTypes = [];
    for (k in rows[1]) {
        var cell = rows[1][k];
        if (cell.value === "") {
            continue;
        }
        var property = properties[cell[colProp]];
        valueTypes[property] = cell.value;
    }
    // console.log('valueTypes:', valueTypes);

    var descriptions = [];
    for (k in rows[2]) {
        var cell = rows[2][k];
        if (cell.value === "") {
            continue;
        }
        var property = properties[cell[colProp]];
        descriptions[property] = cell.value.toUpperCase();
    }
    // console.log('descriptions:', descriptions);

    var syncForceRemote = [];
    for (k in rows[3]) {
        var cell = rows[3][k];
        if (cell.value === "") {
            continue;
        }
        var property = properties[cell[colProp]];
        syncForceRemote[property] = cell.value;
    }
    // console.log('syncForceRemote:', syncForceRemote);

    var defaultValues = [];
    // for (k in rows[3]) {
    //     var cell = rows[3][k];
    //     if (cell.value === "") {
    //         continue;
    //     }
    //     var property = properties[cell[colProp]];
    //     defaultValues[property] = cell.value;
    // }
    // console.log('defaultValues:', defaultValues);

    var others = [];
    for (k in rows[4]) {
        var cell = rows[4][k];
        if (cell.value === "") {
            continue;
        }
        var property = properties[cell[colProp]];
        others[property] = cell.value;
    }
    // console.log('others:', others);

    var gssClass = new Builder.GssClass(sheetName, properties, valueTypes, descriptions, defaultValues, others, syncForceRemote);
    gssClassList[gssClassList.length] = gssClass;
}
