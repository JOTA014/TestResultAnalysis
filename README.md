
# Test Result Analysis Project

This Project will take a specificly structured JSON, export it to a CSV file and display some calculations done by requirement. We have divided this in 2 projects:

- /SolutionForTestResultAnalysis.ConsoleClient: Contains a client (console) application used to execute the different flows

- /SolutionForTestResultAnalysis.Logic: Contains both the data model, a FileHelper and TestRunAnalyzer class. The first one is used to map out json structure while deserialized, the second one is a helper class created to encapsulate a set of file related methods. The third one is where the actual magic happens, here we have the metric calculations, the deserializacion and the CSV file export.


## Tech Stack

**Built using:** .Net 7 and Visual Studio 2022


## App flow
I have created [This PDF](https://drive.google.com/file/d/1-Ddb4PLqzA7bjw0GCQPqgGp0-glNgqA0/view?usp=sharing) with a flow diagram explaining how to use the app.
## Json Structure
This is the JSon structure I assumed for this project. The execution time is calculated in seconds and the test result property is an enum with Pass and Fail options, so please be sure to use those exact values

```json
[
    {
        "id": 1000,
        "name": "Successful login",
        "testResult": "Pass",
        "executionTime": 108,
        "timeStamp": "2023-11-15T13:38:01Z"
    },
    {
        "id": 1001,
        "name": "Login with existing user, wrong password",
        "testResult": "Fail",
        "executionTime": 105,
        "timeStamp": "2023-11-15T13:38:01Z"
    }
]

```
## Input Example

We can execute the program using a json file with a structure just like the one in the [json structure section](##Json-Structure)
## Evidence

- [This screenshot](https://drive.google.com/file/d/1LJx8ZZfgYFwlVORk1FeB5-Zo4WnOh-sd/view?usp=sharing) shows the console app output

- [This screenshot](https://drive.google.com/file/d/1mj1Ls0ZanYWfkXYw3xaoPdEAh32W9bSb/view?usp=sharing) shows the generated file with the above operation

## Challenge
[This is a copy of the instuctions I received over email](https://drive.google.com/file/d/1djJNZs-jCCDjDiUkKXFPH3kAo_9tzocY/view?usp=sharing)
