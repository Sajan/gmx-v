## Introduction ##
The [GMX/V](http://www.lisa.org/Global-information-M.104.0.html) standard defines a standard way to count words and characters within a document and a standard XML format to share this data between applications.

This code has been released to the community by [Gould Tech Solutions](http://www.gouldtechsolutions.com).

### Library ###
The code constitutes a reference implementation of the GMX/V standard, complete with unit tests, written in C#.  It is useful for programmers who use the .Net platform and wish to integrate GMX/V into their products.

![https://gmx-v.googlecode.com/svn/trunk/ProjectScreenshots/TestSuite.png](https://gmx-v.googlecode.com/svn/trunk/ProjectScreenshots/TestSuite.png)

### Console Application ###
A Windows application is available in the downloads section which allows you to retrieve a word count from a given XLIFF document by entering a command line.

To use it, you must access the Windows command prompt.  Example output is shown below:

![https://gmx-v.googlecode.com/svn/trunk/ProjectScreenshots/ExampleOutput.png](https://gmx-v.googlecode.com/svn/trunk/ProjectScreenshots/ExampleOutput.png)

You can store this GMX/V output in a file by using the following command:

```
WordCount.exe my_xliff_file.xlf > C:\gmxv.xml
```