# Performance Time Logging Tool
+ Start > Run > Command Prompt with admin privileges
+ nuget.exe pack -IncludeReferencedProjects -properties Configuration=Release
+ **TimeLogger.Init**("name_of_method");

+ **TimeLogger.Start**("code_block_1");
    + if (USE_V1) {
    + ... //code need to optimize
    + }
    + else {
    + ... //code optimized
    + }
+ **TimeLogger.Stop**("code_block_1");

+ ...

+ **TimeLogger.Start**("code_block_n");
    + if (USE_V1) {
    + ... //code need to optimize
    + }
    + else {
    + ... //code optimized
    + }
+ **TimeLogger.Stop**("code_block_n");

+ **TimeLogger.Summary**();