//console.log("build excel data .. ");

var fs = require("fs");
var path = require('path');
var ncp = require('ncp').ncp;
var Handlebars = require('handlebars');
var Tools = require("./Tools");
const { isNullOrUndefined } = require("util");
var exports = module.exports = {};

// new Map([[1, 'x'], [2, 'y'], [3, 'z']]);
var SubTypeDefine = {
    BP: ["BP","BT"],
    LG: ["LG"],
	AI: ["AI"]
};

Handlebars.registerHelper("quotes", function(str) {
	return str.replace(/\"/gi, "\\\"");
});


// Handlebars.registerHelper('breaklines', function(text) {
//     return text.replace(/(\r\n|\n|\r)/g, '\\/\\/ ');
// });

var unityScriptDir = "./CodeTable/Unity/";
exports.Cleanup = function() {
	Tools.rmDir(unityScriptDir);
	Tools.mkDir(unityScriptDir);
	// console.log('临时目录清空完毕!');
}

exports.OutputConfig = function(unityScriptDir, classTemplate, nameSpace, managerClassTemplate, jsonPath, subTypeStr, name) {
	this.unityScriptDir = unityScriptDir;
	this.classTemplate = classTemplate;
	this.nameSpace = nameSpace;
	this.name = name;
	this.managerClassTemplate = managerClassTemplate;
	this.jsonPath = jsonPath;
	var subType = [];
	var filters = [];

	var ret = [];
	if (subTypeStr) {
		ret = subTypeStr.split(',');
	}

	if (ret.length == 0) {
		// 不过滤，不生成子模块管理器
		this.filterType = 'any';
// 		for (var st in SubTypeDefine) {
// 	    	subType[subType.length] = st;
// 	    	filters.push.apply(filters, SubTypeDefine[st]);
// 　　　　　};
	} else {
		// 过滤，生成子模块管理器
		this.filterType = 'nin';

		ret.forEach(function(st) {
	    	subType[subType.length] = st;
	    	filters.push.apply(filters, SubTypeDefine[st]);
		});
	}

	this.subType = subType;
	this.filters = filters;
	console.log('subType', this.subType);
	console.log('filters', this.filterType, this.filters);
}

///////////////////////////////////// Google Sheets /////////////////////////////////////
//for google spreadsheet
exports.GssClass = function(filename, properties, valueTypes, descriptions, nameSpace, name) {
	this.typeName = filename;
	this.className = upperFirstWord(this.typeName);
	this.filename = filename;
	this.properties = properties;
	this.valueTypes = valueTypes;
	this.descriptions = descriptions;
	this.nameSpace = nameSpace;
	this.name = name;
	this.lowersheetname = filename.toLowerCase();
}

function ManagerGssClass(gssClassList, nameSpace, jsonPath, subType, name) {
	if (name == undefined || name == "") {
		this.managerClassName = nameSpace + "ConfigManager";
	} else {
		this.managerClassName = "ConfigManager";
	}
	this.jsonPath = jsonPath;
	this.nameSpace = nameSpace;
	var idx = 0;
	var fieldArrayTemp = [];
	gssClassList.forEach(function(gssClass)
	{
		fieldArrayTemp[idx] = new ManagerField();
		fieldArrayTemp[idx].configClassName = gssClass.className;
		fieldArrayTemp[idx].lowersheetname = gssClass.lowersheetname;
		fieldArrayTemp[idx].nameSpace = nameSpace;
		++idx;
	});
	this.fieldArray = fieldArrayTemp;

	idx = 0;
	var subTypeArrayTemp = [];
	subType.forEach(function(st)
	{
		subTypeArrayTemp[idx] = new ManagerField();
		subTypeArrayTemp[idx].subType = st;
		++idx;
	});
	this.subTypeArray = subTypeArrayTemp;
	// console.log("========= " + nameSpace);
	// console.log("========= " + jsonPath);
}

exports.buildSpreadsheet = function(gssClassList, outputConfig) {
	//build
	gssClassList.forEach (function(gssClass) {
		buildGssClass(gssClass, outputConfig);
	});

	//check error
	if (buildError.length > 0) {
		console.log("发现" + buildError.length + "个错误: \n");
		for (var i = 0; i < buildError.length; i++) {
			console.log(buildError[i]);
		};
	} else {
		// console.log("解析完成");
	}

	buildConfigManager(gssClassList, outputConfig);
		
	//copy
	mkDir(outputConfig.unityScriptDir);
		
	ncp(unityScriptDir, outputConfig.unityScriptDir, function(err) {
		if (err) {
			return console.error(err);
		}
		// console.log('复制Unity Script文件至目标路径!');
		Tools.rmDir(unityScriptDir);
	});


}


function ManagerField() {
	this.configClassName = "";
	this.lowersheetname = "";
}

function Field() {
	this.fieldName = "";
	this.idIndex = 0; //field顺序
	this.type = "";
	this.typeUpperCase = "";
	this.retType = "int";
	this.realType = "";
	this.getterName = "";
	this.desc = "";
	this.defaultValue = undefined;
	this.defaultIsNew = false;
	this.hasDefaultValue = false;
	this.isProtectedValue = false;

	this.setFieldName = function(name) {
		this.fieldName = name;
		this.getterName = name[0].toUpperCase() + name.substring(1);
	}

	this.setType = function(t) {
		this.type = t;
		this.realType = t;

		// console.log('this.type:', this.type);
		this.typeUpperCase = this.type.toUpperCase();
		if(this.realType.toLowerCase() == "number")
		{
			this.realType = "int";
			return true;
		}
		else if(this.realType.toLowerCase() == "int")
		{
			this.realType = "int";
			return true;
		}
		else if (this.realType.toLowerCase() == "bool")
		{
			this.realType = "bool";
			return true;
		}
		else if (this.realType.toLowerCase() == "string")
		{
			this.realType = "string";
			return true;
		}
		else if (this.realType.toLowerCase() == "arraynumber")
		{
			this.realType = "List<int>";
			return true;
		}
		else if (this.realType.toLowerCase() == "stringnumber")
		{
			this.realType = "List<string>";
			return true;
		}
		else if (this.realType.toLowerCase() == "arraystring")
		{
			this.realType = "List<string>";
			return true;
		}
		else if (this.realType.toLowerCase() == "arrayfloat")
		{
			this.realType = "List<float>";
			return true;
		}
		else if (this.realType.toLowerCase() == "range")
		{
			this.realType = "List<float>";
			return true;
		}
		else if (this.realType.toLowerCase() == "float")
		{
			this.realType = "float";
			return true;
		}
		else if (this.realType.toLowerCase() == "double")
		{
			this.realType = "double";
			return true;
		}
		else if (this.realType.toLowerCase() == "long")
		{
			this.realType = "long";
			return true;
		}
		else if (this.realType.toLowerCase() == "arraystring2")
		{
			this.realType = "List<List<string>>";
			return true;
		}
		else
		{
			return false;
		}
	}
}
var buildError = [];

function addError(table, msg) {
	console.log(msg);
	buildError[buildError.length] = table + " -> " + msg
}

function buildGssClass (gssClass, outputConfig) {
	//console.log(gssClass.contentList);
	// console.log("export google spreadsheet: " , gssClass.filename);
	var fieldArray = [];
	var useSplit = false;
	var useSplitSub = false;
	var idxCol = 0;
	for(var key in gssClass.properties) {
		// if(key == 1)
		// {
		// 	continue;
		// }
		var property = gssClass.properties[key];
		// console.log(" =========== propertys " );
		// console.log(property);
		// console.log(gssClass.valueTypes);
		if (! fieldArray[idxCol]) {
			fieldArray[idxCol] = new Field();
			fieldArray[idxCol].idIndex = idxCol;
			fieldArray[idxCol].setFieldName(property);
			fieldArray[idxCol].desc = gssClass.descriptions[property] ? gssClass.descriptions[property].replace(/(\r\n|\n|\r)/g, '\; ') : "";
			var setTypeSuccess = fieldArray[idxCol].setType(gssClass.valueTypes[property]);
			if (setTypeSuccess)
			{
				// console.log(fieldArray);
				idxCol++;
			}
			else
			{
				fieldArray.splice(idxCol, 1);
				// console.log('error type ');
				// console.log(fieldArray);
			}
		}
	}
	// console.log(outputConfig.classTemplate);
	var contentUnity = Handlebars.compile(fs.readFileSync(outputConfig.classTemplate, "utf-8"));
	//class file
	var codeFile = unityScriptDir + gssClass.nameSpace + gssClass.className + ".cs";
	// console.log("codeFile:", codeFile);
	console.log("nameSpace==============:", gssClass.nameSpace);
	var code = contentUnity({
		className: gssClass.className,
		nameSpace: gssClass.nameSpace,
		name: gssClass.name,
		fieldArray: fieldArray,
		useSplit: (useSplit||useSplitSub)
	});
	fs.writeFileSync(codeFile, code, "utf-8");
	//console.log("gss: ", fieldArray);
    console.log('generate -', gssClass.className + ".cs");
}

function buildConfigManager(gssClassList, outputConfig)
{
	var managerGss = new ManagerGssClass(gssClassList, outputConfig.nameSpace, outputConfig.jsonPath, outputConfig.subType, outputConfig.name);
	var contentUnity = Handlebars.compile(fs.readFileSync(outputConfig.managerClassTemplate, "utf-8"));
	var codeFile = unityScriptDir + managerGss.managerClassName + ".Loader" + ".cs";
	var code = contentUnity({
		managerClassName: managerGss.managerClassName,
		nameSpace: outputConfig.nameSpace,
		name: outputConfig.name,
		fieldArray: managerGss.fieldArray,
		subTypeArray: managerGss.subTypeArray,
		jsonPath: outputConfig.jsonPath,
		useSplit: false
	});
	fs.writeFileSync(codeFile, code, "utf-8");
    console.log('generate +', managerGss.managerClassName + ".Loader" + ".cs");
}

function to10Radix(data) {
	var ret = 0;
	for (var i = 0; i < data.length; i++) {
		ret += i * 26 + data[i].charCodeAt() - 'A'.charCodeAt();
	}
	return ret;
}

function upperFirstWord(val) {
	// val = val.toLowerCase();
	var ret = "";
	var upper = true;
	for (var i = 0; i < val.length; i++) {
		var c = val[i];
		if (upper) {
			upper = false;
			ret += c.toUpperCase();
		} else {
			if (c == '_') {
				upper = true;
			} else {
				ret += c;
			}
		}
	}
	return ret;
}

/**数值判断。允许数值面前增加+ -号，允许存在一个小数点。输入非法字符返回0
 * 使用：isNumber(this)
 */
function isNumber(str) {
	if(str == undefined) {
		return false;
	}

	var oneDecimal = false;
	var oneChar = 0;
	for (var i = 0; i < str.length; i++) {
		oneChar = str.charAt(i).charCodeAt(0);

		if (oneChar == 45) {
			if (i == 0) {
				continue;
			} else {
				return false;
			}
		}
		// 小数点 
		if (oneChar == 46) {
			if (!oneDecimal) {
				oneDecimal = true;
				continue;
			} else {
				return false;
			}
		}
		// 数字只能在0和9之间 
		if (oneChar < 48 || oneChar > 57) {
			return false;
		}
	}
	return true;
}

function startsWith(str, prefix) {
	return str.indexOf(prefix) === 0;
}

/**
 * 去掉末尾的数字和下划线
 */
function carveEndNum(str) {
	var index = str.length;
	for (var i = str.length - 1; i >= 0; i--) {
		if (isNumber(str.charAt(i))) { // || str.charAt(i) == '_'
			index--;
		}
	}
	return str.substring(0, index);
}

function carveStarStr(str) {
	var index = 0;
	for (var i = 0; i < str.length; i++) {
		if (!isNumber(str.charAt(i))) {
			index++;
		}
	}
	return str.substring(index);
}

exports.joinDir = joinDir = function(dirPath, filename) {
	return path.join(dirPath, filename);
}