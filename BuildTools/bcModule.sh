#!/bin/sh

projectDir="XProject"

modulesAll=(
	{"Game","1z1VPg9sAJzwtPuCtie74igDLIUnGgLuOg39JvVodqcM"}
)






######################################
# Do not modify
######################################
num=${#modulesAll[*]}
argCount=2

if [ "$((num%${argCount}))" != "0" ]; then
	echo "error! declare of module has error, please check."
	exit
fi

moduleCount=$((num/${argCount}))
# echo "moduleCount:" ${moduleCount}

isNum(){
	# read -p "please input a num: "  num
	if echo $1 | grep -q '[^0-9]'; then
		echo false
	else
		echo true
	fi
}

listAll(){
	echo "==============================================="
	echo "Bad argument:" $1
	echo "Please use the available modules as shown below"
	echo "==============================================="
	echo "0 - all"
	for ((i = 0; i < $moduleCount; i++)); do
	 	idx=$i*${argCount}
 		name=${modulesAll[$idx]}
 		nameLower=$(echo $name | tr '[A-Z]' '[a-z]')
 		echo $(expr $i + 1) "-" $nameLower
	done
}

idxBegin=-1
idxEnd=-1

if [ "$#" == "1" ] ; then
	num=`isNum $1`
	if [ $num == "true" ]; then
		if [ $1 == 0 ]; then
			idxBegin=0
			idxEnd=$moduleCount
		else
			if [ $1 -gt $moduleCount ] || [ $1 -lt 1 ]; then
				listAll $1
				exit
	 		fi
			idxBegin=$(expr $1 - 1)
			idxEnd=$1
		fi
	else
		for ((i = 0; i < $moduleCount; i++)); do
		 	idx=$i*${argCount}
	 		name=${modulesAll[$idx]}
	 		nameLower=$(echo $name | tr '[A-Z]' '[a-z]')
	 		argLower=$(echo $1 | tr '[A-Z]' '[a-z]')
		 	if [ "$argLower" == "$nameLower" ]; then
				idxBegin=$i
				idxEnd=$(expr $i + 1)
				break
	 		fi
		done
 	fi
fi
if [ "$idxBegin" -lt "0" ] ; then
	listAll $1
	exit
fi

# echo "idxBegin:" ${idxBegin}
# echo "idxEnd:" ${idxEnd}

# cd $(dirname $0)
rootDir=`pwd`/.Tools
classTemplate="${rootDir}/Template/Config.template"
managerClassTemplate="${rootDir}/Template/ConfigManagerSplit.template"
projectDir="${rootDir}/../../${projectDir}/"

for ((i = idxBegin; i < $idxEnd; i++)); do
 	idx=$i*${argCount}
 	name=${modulesAll[$idx]}
 	nameLower=$(echo $name | tr '[A-Z]' '[a-z]')
	nameSpace=${name}
	unityScriptDir="${projectDir}Assets/Scripts/Common/Config/${name}/CodeGen"
	jsonPath="Configs/${name}"
 	spreadsheet_id=${modulesAll[$idx+1]}

	node ${rootDir}/Js/Tools.js ${unityScriptDir} --meta --mk
	node ${rootDir}/Js/gDocBuilder.js ${spreadsheet_id} ${unityScriptDir} ${classTemplate} ${nameSpace} ${managerClassTemplate} ${jsonPath}
done

echo "Finished"