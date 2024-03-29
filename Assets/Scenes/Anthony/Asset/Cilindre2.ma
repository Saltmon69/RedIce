//Maya ASCII 2022 scene
//Name: Cilindre2.ma
//Last modified: Thu, Mar 21, 2024 03:03:56 PM
//Codeset: 1252
requires maya "2022";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2022";
fileInfo "version" "2022";
fileInfo "cutIdentifier" "202108111415-612a77abf4";
fileInfo "osv" "Windows 10 Home v2009 (Build: 19045)";
fileInfo "UUID" "4FE95150-40A4-C220-B4D6-4FA674D5430A";
createNode transform -n "pCylinder1";
	rename -uid "459CB892-40CF-6742-E184-088273BA3FFD";
	setAttr ".t" -type "double3" 0 1.4483671741775601 0 ;
	setAttr ".r" -type "double3" 90 0 0 ;
	setAttr ".s" -type "double3" 1 3.9320648195602934 1 ;
createNode mesh -n "pCylinderShape1" -p "pCylinder1";
	rename -uid "E3B3CBF9-4DB6-6910-07EE-09BA8F0D5DB3";
	setAttr -k off ".v";
	setAttr -s 2 ".iog[0].og";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.49999988079071045 0.50046992301940918 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode groupId -n "groupId1";
	rename -uid "C97D9628-412C-D1CD-52F9-FA9B8399A6D4";
	setAttr ".ihi" 0;
createNode objectSet -n "set1";
	rename -uid "2F638436-4B72-0A21-F447-56897C101DBC";
	setAttr ".ihi" 0;
createNode deleteComponent -n "deleteComponent1";
	rename -uid "D74E026D-4317-F1B5-52E4-3592822D0BB1";
	setAttr ".dc" -type "componentList" 1 "f[20:59]";
createNode polyTweak -n "polyTweak1";
	rename -uid "C98E5061-485C-49AE-EA1F-B7B256A39721";
	setAttr ".uopa" yes;
	setAttr -s 42 ".tk";
	setAttr ".tk[0]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[1]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[2]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[3]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[4]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[5]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[6]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[7]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[8]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[9]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[10]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[11]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[12]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[13]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[14]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[15]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[16]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[17]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[18]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[19]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[40]" -type "float3" 0 -0.83564794 0 ;
	setAttr ".tk[62]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[63]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[64]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[65]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[66]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[67]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[68]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[69]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[70]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[71]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[72]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[73]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[74]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[75]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[76]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[77]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[78]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[79]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[80]" -type "float3" 0 0.053868212 0 ;
	setAttr ".tk[81]" -type "float3" 0 0.053868212 0 ;
createNode groupParts -n "groupParts1";
	rename -uid "6E6ACACE-47E8-66BE-1F91-D7AA1A07DEDF";
	setAttr ".ihi" 0;
	setAttr ".ic" -type "componentList" 2 "e[0:39]" "e[60:99]";
createNode polySplit -n "polySplit2";
	rename -uid "885BB4E2-4D90-6FA4-1A79-5C94CBA109E2";
	setAttr -s 21 ".e[0:20]"  0.407323 0.407323 0.407323 0.407323 0.407323
		 0.407323 0.407323 0.407323 0.407323 0.407323 0.407323 0.407323 0.407323 0.407323
		 0.407323 0.407323 0.407323 0.407323 0.407323 0.407323 0.407323;
	setAttr -s 21 ".d[0:20]"  -2147483608 -2147483607 -2147483606 -2147483605 -2147483604 -2147483603 
		-2147483602 -2147483601 -2147483600 -2147483599 -2147483598 -2147483597 -2147483596 -2147483595 -2147483594 -2147483593 -2147483592 -2147483591 
		-2147483590 -2147483589 -2147483608;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit1";
	rename -uid "4114C4E6-4928-D739-432B-DCB236654E29";
	setAttr -s 21 ".e[0:20]"  0.73288 0.73288 0.73288 0.73288 0.73288 0.73288
		 0.73288 0.73288 0.73288 0.73288 0.73288 0.73288 0.73288 0.73288 0.73288 0.73288 0.73288
		 0.73288 0.73288 0.73288 0.73288;
	setAttr -s 21 ".d[0:20]"  -2147483608 -2147483607 -2147483606 -2147483605 -2147483604 -2147483603 
		-2147483602 -2147483601 -2147483600 -2147483599 -2147483598 -2147483597 -2147483596 -2147483595 -2147483594 -2147483593 -2147483592 -2147483591 
		-2147483590 -2147483589 -2147483608;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polyCylinder -n "polyCylinder1";
	rename -uid "ECD3842C-4C62-7AE6-B4BD-F08C1FB41304";
	setAttr ".sc" 1;
	setAttr ".cuv" 3;
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
	setAttr ".fprt" yes;
select -ne :renderPartition;
	setAttr -s 2 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 5 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :defaultColorMgtGlobals;
	setAttr ".cfe" yes;
	setAttr ".cfp" -type "string" "<MAYA_RESOURCES>/OCIO-configs/Maya2022-default/config.ocio";
	setAttr ".vtn" -type "string" "ACES 1.0 SDR-video (sRGB)";
	setAttr ".vn" -type "string" "ACES 1.0 SDR-video";
	setAttr ".dn" -type "string" "sRGB";
	setAttr ".wsn" -type "string" "ACEScg";
	setAttr ".otn" -type "string" "ACES 1.0 SDR-video (sRGB)";
	setAttr ".potn" -type "string" "ACES 1.0 SDR-video (sRGB)";
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
connectAttr "groupId1.id" "pCylinderShape1.iog.og[0].gid";
connectAttr "set1.mwc" "pCylinderShape1.iog.og[0].gco";
connectAttr "deleteComponent1.og" "pCylinderShape1.i";
connectAttr "groupId1.msg" "set1.gn" -na;
connectAttr "pCylinderShape1.iog.og[0]" "set1.dsm" -na;
connectAttr "polyTweak1.out" "deleteComponent1.ig";
connectAttr "groupParts1.og" "polyTweak1.ip";
connectAttr "polySplit2.out" "groupParts1.ig";
connectAttr "groupId1.id" "groupParts1.gi";
connectAttr "polySplit1.out" "polySplit2.ip";
connectAttr "polyCylinder1.out" "polySplit1.ip";
connectAttr "pCylinderShape1.iog" ":initialShadingGroup.dsm" -na;
// End of Cilindre2.ma
