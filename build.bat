@echo off

REM Released under the MIT License.

REM Copyright (c) 2024 Jeesu Choi

REM Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
REM documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
REM rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
REM persons to whom the Software is furnished to do so, subject to the following conditions:

REM The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
REM Software.

REM THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
REM WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
REM COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
REM OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

powershell -executionpolicy remotesigned -File %~dp0\build.ps1 %*