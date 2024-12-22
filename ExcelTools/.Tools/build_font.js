var fs = require("fs");
var shell = require("shelljs");
var util = require("util");

var gdcl = "/tools/Glyph\\ Designer.app/Contents/MacOS/Glyph\\ Designer";
var pngquant = "../../../../../CookingJourney/scripts/tools/pngquant/pngquant --skip-if-larger --quality 20 ";
var font_path = "/uioutput/common2/picture/game_font";
// gdcl usage: see ./tools/Glyph Designer.app/Contents/MacOS/Glyph Designer -h

function trim_space_and_color(str) {
    // trim the [colro=#12345] [/color] and `space` '\n'
    return str.replace(/\[color=#[0-9]*]/g, "").replace(/\[\/color]/g, "").replace(/\n|\r/g, "").replace(/ /g, "");
}

function execute_cmd(cmd) {
    var code = shell.exec(cmd);
    if (code != 0) {
        console.log("error build for cmd: ", cmd);
        process.exit(-1);
    }
}

function append_json_to_content(file_path, content) {
    var json_file_content = fs.readFileSync(file_path);
    var parsed_json = JSON.parse(json_file_content);

    for (var i in parsed_json) {
        var entry = parsed_json[i];
        if (entry.value && entry.value.length > 0) {
            var value = trim_space_and_color(entry.value);
            content.push(value);
        }
    }
}

function array_equal(a, b) {
    if (a.length != b.length) {
        return false;
    }

    a.sort();
    b.sort();

    for (var i = 0; i < a.length; ++i) {
        if (a[i] != b[i]) {
            return false;
        }
    }
    return true;
}

function parse_fnt_file(fnt_content) {
    var ret = {
        count: 0,
        characters : []
    };

    ret.count = parseInt(fnt_content[3].replace("chars count=", ""));
    var start = 4;
    var end = start + ret.count;
    for (var i = start; i < end; ++i) {
        var id = parseInt(fnt_content[i].replace("char id=", ""));
        ret.characters.push(id);
    }

    return ret;
}

function validate_fnt(origin_fnt_file_name, existed_fnt_file_name) {
    var origin = fs.readFileSync(origin_fnt_file_name).toString().split("\n");
    var existed = fs.readFileSync(existed_fnt_file_name).toString().split("\n");

    var origin_fnt_chars = parse_fnt_file(origin);
    var existed_font_chars = parse_fnt_file(existed);

    return !array_equal(origin_fnt_chars.characters, existed_font_chars.characters);
}

function deleteRepetion(arr){
    var arrTable = {},arrData = [];
    for (var i = 0; i < arr.length; i++) {
        if( !arrTable[ arr[i] ]){
            arrTable[ arr[i] ] = true;
            arrData.push(arr[i])
        }
    }
    return arrData;
}

function build(output_path_array, locale_file_path, language_code) {
    // How we build the fonts and what is the outputs.
    var root = shell.pwd();
    // parse data from json and write them to text.
    var text_contents = [];

    var file_path = util.format("%s/locale_%s.json", locale_file_path, language_code);
    var file_path_loading = util.format("%s/locale_loading_%s.json", locale_file_path, language_code);
    append_json_to_content(file_path, text_contents);
    append_json_to_content(file_path_loading, text_contents);


    var text1 = text_contents.join();
    var text = deleteRepetion(text1);



    // This is temp, we would optimize it later
    text += "abcdefghijklmnopqrstuvwxyz1234567890-ABCDEFGHIJKLMNOPQRSTUVWXYZ!#@#$%^&*()[];',../\\{}:<>?_";
    var in_file = output_path_array + language_code + ".txt";
    fs.writeFileSync(in_file, text);

    // return where we belong
    shell.cd(root);
}



function main(args) {
    // args[0] is node binaray path
    // args[1] is then file path of this file
    // so we read start from args[2]
    var output_path_array = args[2];
    var translation_json_root_path = args[3];
    var languages = eval(args[4]);

    console.log("output_path = ", output_path_array);
    console.log("translation_json_path = ", translation_json_root_path);
    console.log("languages = ", languages);

    for (var i = 0; i < languages.length; ++i) {
        var language_code = languages[i];
        build(output_path_array, translation_json_root_path, language_code);
    }
}

main(
    [1, 2,  // place holder
    ["../TripleMatch2/Assets/I18n/"],
    "../TripleMatch2/Assets/Export/Configs/LocaleConfig",
    "[\"en\",\"de\",\"fr\",\"jp\", \"kr\", \"zh\", \"zht\", \"pt\", \"es\", \"ru\", \"it\", \"nl\", \"tr\", \"id\", \"th\", \"vi\"]"
    ]
);
// main(process.argv);
