#!/usr/bin/env node
var fs = require('fs');
var path = require('path');
var program = require('commander');
var Builder = require('./BuilderUtilsBP');
var getTable = require('./getTable');

program
    .version('0.0.1')
    .usage('<spreadsheet-id> <unity-script-dir> <unity-script-template> [options]')
    .parse(process.argv);

if (program.args.length < 3) {
    program.help();
}

var TranspositionTable = {
    "Scene1": true,
    "Timeline": true,
    "Timeline-a": true,
    "Timeline_b": true,
    "Timeline-c": true,
    "Timeline-d": true,
    "Scene1-a": true,
    "Global": true,
    "MapTimeLine": true
};

var spreadsheetId = program.args[0];
var outputConfig = new Builder.OutputConfig(program.args[1], program.args[2], program.args[3], program.args[4], program.args[5], program.args[6]);

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
        if (outputConfig.filterType == 'any') {
            readsheet(sheetName, content[sheetName], ++i);
        } else if (outputConfig.filterType == 'in' && filterContains(sheetName, outputConfig.filters)) {
            readsheet(sheetName, content[sheetName], ++i);
        } else if (outputConfig.filterType == 'nin' && !filterContains(sheetName, outputConfig.filters)) {
            readsheet(sheetName, content[sheetName], ++i);
        }
    }

    Builder.buildSpreadsheet(gssClassList, outputConfig);
    running = false;
    // console.log('google spreadsheet finished!');
});

//queue to build
var gssClassList = [];
var running = false;

function filterContains(s, filters){
    // console.log('filterContains1:', s, filters);
    for (i in filters) {
        if (s.startsWith(filters[i])) {
    // console.log('filterContains2:', s, filters);
            return true;
        }
    }
    return false;
}

//NOTE: use spreadsheet NOT spreadsheets
function transpositionTable(rows){
    var nrow = 0;
    var ncol = 0;
    nrow = rows.length;
    ncol = rows[0].length;
    var newRows = new Array(ncol);

    for(var c = 0;c<ncol;++c) {
        newRows[c] = [];
    }
    for(var c = 0;c<ncol;++c) {
        for(var r = 0;r<nrow;++r){
            var cell = rows[r][c];
            if(cell==null){
                cell = {row:r,col:c,value:""};
                //console.log("+++++++++++",r,c);
            }
            if(newRows[cell.col][cell.row]==null){
                newRows[cell.col][cell.row] = cell;
                if(cell!=null && cell.row !=null){
                    var tmp = cell.row;
                    cell.row = cell.col;
                    cell.col = tmp;
                }
            }
        }
    }
    return newRows;
}

function readsheet(sheetName, sheetData, sheetIndex) {
    // console.log('google spreadsheet: read the ', sheetIndex, ' ', 'sheet: ', sheetName);
    //console.log('sheet: ', worksheets[idxSheet]);

    // var jsTemplate = requireJsTemplate();
    // var customerJs = requireCustomerJs();
    var bTranspositionTable = false;
    if(TranspositionTable && TranspositionTable[sheetName]) {
        bTranspositionTable = true;
    }

    var jsTemplateMap = {};

    var rowProp = program.vertical ? "col" : "row";
    var colProp = program.vertical ? "row" : "col";

    var rows = [];
    var datas = sheetData;
    for (rowIndex in datas) {
        // console.log('rowIndex:', rowIndex);
        if (typeof rows[rowIndex] === "undefined") {
            rows[rowIndex] = [];
        }
        var rowdata = datas[rowIndex];
        // console.log('rowdata:', rowdata);
        for (colIndex in rowdata) {
            rows[rowIndex].push({row:rowIndex,col:colIndex,value:rowdata[colIndex]});
        };
    };

    //console.log("===============")
    if(bTranspositionTable){
        rows = transpositionTable(rows);
    }

    rows.forEach(function(col) {
        col.sort(function(cell1, cell2) {
            return cell1[colProp] - cell2[colProp];
        });
    });

    var finalList = [];

    var properties = [];
    for (k in rows[0]) {
        var val = rows[0][k].value;
        properties.push(val.charAt(0).toUpperCase() + val.slice(1));
    }

    var valueTypes = [];
    for (k in rows[1]) {
        var cell = rows[1][k];
        if (cell.value === "") {
            continue;
        }
        var property = properties[cell[colProp]];
        valueTypes[property] = cell.value;
    }

    var descriptions = [];
    for (k in rows[2]) {
        var cell = rows[2][k];
        if (cell.value === "") {
            continue;
        }
        var property = properties[cell[colProp]];
        descriptions[property] = cell.value.toUpperCase();
    }

    var gssClass = new Builder.GssClass(sheetName, properties, valueTypes, descriptions, program.args[3]);
    gssClassList[gssClassList.length] = gssClass;
}

// function requireJsTemplate(){
//     console.log("requireJsTemplate")
//     var jsTemplate = {};
//     var files = fs.readdirSync("./JsTemplate");
//     for(var index in files){
//         console.log(files[index]);
//         if(!files[index].endsWith(".js")) {
//             continue;
//         }
//         var name = path.basename(files[index], ".js");
//         var template = require(path.join("../JsTemplate", name));
//         var md = new template ();
//         jsTemplate[name] = md;
//         jsTemplate[name.toUpperCase()] = md;
//     }
//     return jsTemplate;
// }

// function requireCustomerJs(){
//     console.log("requireCustomerJs")
//     var customerJsArr = {};
//     var files = fs.readdirSync("./CustomerJs");
//     for(var index in files){
//         console.log(files[index])
//         var name = path.basename(files[index],".js");
//         var md = require(path.join("../CustomerJs",name));
//         customerJsArr[name] = md;
//         customerJsArr[name.toUpperCase()] = md;
//     }
//     return customerJsArr;
// }
