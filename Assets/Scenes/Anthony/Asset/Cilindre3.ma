//Maya ASCII 2022 scene
//Name: Cilindre3.ma
//Last modified: Fri, Mar 22, 2024 11:18:25 AM
//Codeset: 1252
requires maya "2022";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2022";
fileInfo "version" "2022";
fileInfo "cutIdentifier" "202108111415-612a77abf4";
fileInfo "osv" "Windows 10 Home v2009 (Build: 19045)";
fileInfo "UUID" "DCA0442B-486F-F606-2A4A-469C25764A05";
createNode transform -n "pCylinder1";
	rename -uid "A565B755-46AA-F15E-240F-EDBB5B725775";
	setAttr ".t" -type "double3" 0 1.9138736403963303 0 ;
	setAttr ".r" -type "double3" 90 0 0 ;
	setAttr ".s" -type "double3" 0.28722771468248459 0.51849011835745218 0.28722771468248459 ;
createNode mesh -n "pCylinderShape1" -p "pCylinder1";
	rename -uid "97206093-4E9E-9EFD-83FE-FDB4F90D229F";
	setAttr -k off ".v";
	setAttr -s 2 ".iog[0].og";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.49999988079071045 0.61795112490653992 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode groupId -n "groupId1";
	rename -uid "45766436-47C2-D0F6-4B2E-0CA82A512038";
	setAttr ".ihi" 0;
createNode objectSet -n "set1";
	rename -uid "C7125C9D-4FE9-AE6A-5C29-5E93DC19DBB4";
	setAttr ".ihi" 0;
createNode polySplit -n "polySplit7";
	rename -uid "848F76E3-44E9-12A4-6BFD-8CB08ECEAEFD";
	setAttr -s 21 ".e[0:20]"  0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5
		 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5;
	setAttr -s 21 ".d[0:20]"  -2147483608 -2147483607 -2147483606 -2147483605 -2147483604 -2147483603 
		-2147483602 -2147483601 -2147483600 -2147483599 -2147483598 -2147483597 -2147483596 -2147483595 -2147483594 -2147483593 -2147483592 -2147483591 
		-2147483590 -2147483589 -2147483608;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit6";
	rename -uid "23CB0095-4775-DF1F-9A16-8ABC548118D3";
	setAttr -s 21 ".e[0:20]"  0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5
		 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5;
	setAttr -s 21 ".d[0:20]"  -2147483548 -2147483529 -2147483530 -2147483531 -2147483532 -2147483533 
		-2147483534 -2147483535 -2147483536 -2147483537 -2147483538 -2147483539 -2147483540 -2147483541 -2147483542 -2147483543 -2147483544 -2147483545 
		-2147483546 -2147483547 -2147483548;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit5";
	rename -uid "02A250B3-40A6-A37E-A03A-11A106699496";
	setAttr -s 21 ".e[0:20]"  0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5
		 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5;
	setAttr -s 21 ".d[0:20]"  -2147483588 -2147483587 -2147483586 -2147483585 -2147483584 -2147483583 
		-2147483582 -2147483581 -2147483580 -2147483579 -2147483578 -2147483577 -2147483576 -2147483575 -2147483574 -2147483573 -2147483572 -2147483571 
		-2147483570 -2147483569 -2147483588;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit4";
	rename -uid "4E62BD88-4A1D-2B95-83BD-AA9B31A9B7EA";
	setAttr -s 21 ".e[0:20]"  0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5
		 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5;
	setAttr -s 21 ".d[0:20]"  -2147483508 -2147483507 -2147483506 -2147483505 -2147483504 -2147483503 
		-2147483502 -2147483501 -2147483500 -2147483499 -2147483498 -2147483497 -2147483496 -2147483495 -2147483494 -2147483493 -2147483492 -2147483491 
		-2147483490 -2147483489 -2147483508;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit3";
	rename -uid "19EFCBE0-4DB4-B30D-FE4B-06B7742A6C55";
	setAttr -s 21 ".e[0:20]"  0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5
		 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5;
	setAttr -s 21 ".d[0:20]"  -2147483588 -2147483587 -2147483586 -2147483585 -2147483584 -2147483583 
		-2147483582 -2147483581 -2147483580 -2147483579 -2147483578 -2147483577 -2147483576 -2147483575 -2147483574 -2147483573 -2147483572 -2147483571 
		-2147483570 -2147483569 -2147483588;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit2";
	rename -uid "25CCBCB3-48D1-8447-5D05-18A97E20D27D";
	setAttr -s 21 ".e[0:20]"  0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5
		 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5;
	setAttr -s 21 ".d[0:20]"  -2147483608 -2147483607 -2147483606 -2147483605 -2147483604 -2147483603 
		-2147483602 -2147483601 -2147483600 -2147483599 -2147483598 -2147483597 -2147483596 -2147483595 -2147483594 -2147483593 -2147483592 -2147483591 
		-2147483590 -2147483589 -2147483608;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit1";
	rename -uid "18982FD3-4711-5F8F-B328-019D2AB6E5AE";
	setAttr -s 21 ".e[0:20]"  0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5
		 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5 0.5;
	setAttr -s 21 ".d[0:20]"  -2147483608 -2147483607 -2147483606 -2147483605 -2147483604 -2147483603 
		-2147483602 -2147483601 -2147483600 -2147483599 -2147483598 -2147483597 -2147483596 -2147483595 -2147483594 -2147483593 -2147483592 -2147483591 
		-2147483590 -2147483589 -2147483608;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polyTweak -n "polyTweak1";
	rename -uid "CC77D89F-426C-5426-E0C4-0FB97DCCC776";
	setAttr ".uopa" yes;
	setAttr -s 21 ".tk";
	setAttr ".tk[20]" -type "float3" -0.76729208 9.6227741 0.24931352 ;
	setAttr ".tk[21]" -type "float3" -0.65269756 9.6227741 0.47420949 ;
	setAttr ".tk[22]" -type "float3" -0.47420979 9.6227741 0.65269721 ;
	setAttr ".tk[23]" -type "float3" -0.24931379 9.6227741 0.76729172 ;
	setAttr ".tk[24]" -type "float3" -9.6176194e-08 9.6227741 0.80678469 ;
	setAttr ".tk[25]" -type "float3" 0.24931355 9.6227741 0.76729161 ;
	setAttr ".tk[26]" -type "float3" 0.47420943 9.6227741 0.65269709 ;
	setAttr ".tk[27]" -type "float3" 0.65269709 9.6227741 0.47420931 ;
	setAttr ".tk[28]" -type "float3" 0.76729149 9.6227741 0.2493134 ;
	setAttr ".tk[29]" -type "float3" 0.80678451 9.6227741 -1.4426384e-07 ;
	setAttr ".tk[30]" -type "float3" 0.76729149 9.6227741 -0.24931376 ;
	setAttr ".tk[31]" -type "float3" 0.65269697 9.6227741 -0.47420961 ;
	setAttr ".tk[32]" -type "float3" 0.47420931 9.6227741 -0.65269721 ;
	setAttr ".tk[33]" -type "float3" 0.24931346 9.6227741 -0.76729172 ;
	setAttr ".tk[34]" -type "float3" -7.2131918e-08 9.6227741 -0.80678469 ;
	setAttr ".tk[35]" -type "float3" -0.24931361 9.6227741 -0.76729161 ;
	setAttr ".tk[36]" -type "float3" -0.47420943 9.6227741 -0.65269721 ;
	setAttr ".tk[37]" -type "float3" -0.65269709 9.6227741 -0.47420955 ;
	setAttr ".tk[38]" -type "float3" -0.76729149 9.6227741 -0.2493137 ;
	setAttr ".tk[39]" -type "float3" -0.80678451 9.6227741 -1.4426384e-07 ;
createNode deleteComponent -n "deleteComponent1";
	rename -uid "291675FC-4AEA-B75B-29E7-D2B2B0D4A401";
	setAttr ".dc" -type "componentList" 1 "f[20:59]";
createNode groupParts -n "groupParts1";
	rename -uid "FD679565-46D0-D291-3875-089CFAC567EB";
	setAttr ".ihi" 0;
	setAttr ".ic" -type "componentList" 2 "e[0:39]" "e[60:99]";
createNode polyCylinder -n "polyCylinder1";
	rename -uid "FE309FC4-4DB6-E748-0E16-DDAAF01FC8D2";
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
connectAttr "polySplit7.out" "pCylinderShape1.i";
connectAttr "groupId1.msg" "set1.gn" -na;
connectAttr "pCylinderShape1.iog.og[0]" "set1.dsm" -na;
connectAttr "polySplit6.out" "polySplit7.ip";
connectAttr "polySplit5.out" "polySplit6.ip";
connectAttr "polySplit4.out" "polySplit5.ip";
connectAttr "polySplit3.out" "polySplit4.ip";
connectAttr "polySplit2.out" "polySplit3.ip";
connectAttr "polySplit1.out" "polySplit2.ip";
connectAttr "polyTweak1.out" "polySplit1.ip";
connectAttr "deleteComponent1.og" "polyTweak1.ip";
connectAttr "groupParts1.og" "deleteComponent1.ig";
connectAttr "polyCylinder1.out" "groupParts1.ig";
connectAttr "groupId1.id" "groupParts1.gi";
connectAttr "pCylinderShape1.iog" ":initialShadingGroup.dsm" -na;
// End of Cilindre3.ma
