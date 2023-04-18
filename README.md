<h1 align="center">AWS-Token-Refresh</h1> 



## About

A **.NET application** that allow users to fetch security access tokens upon successful authentication with **AWS IAM Identity Center** and update credential files automatically on personal computers or servers

## Overview
![image](https://user-images.githubusercontent.com/65143821/232868182-62605013-a8f1-4dce-85da-d51b913cb7fa.png)

## How to run?


- First, You can either choose `.NET 5.0 Core version` or `.NET 4.5 Framework version` based on the current .NET installation on your local machine.
- Second, `git clone` this repository and customize some of the AWS parameters in `program.cs`.
- Third, `build` the code in `release mode` and `run` the program. (You can also `publish` the code via Visual Studio)
- The program will then open a browser automatically and you can enter your personal `AWS UserID` and `AWS Password` in the browser. 
- As soon as you finish verifying your identity, your credentials will be automatically write into the `.aws file`. (Normally, you can find this file under your `USERNAME` directory)

## Programming Language & Framework & IDE & Cloud

<img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg" width=48px height=48px/> <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dot-net/dot-net-plain-wordmark.svg" width=48px height=48px/> <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dotnetcore/dotnetcore-original.svg" width=48px height=48px/>  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/visualstudio/visualstudio-plain.svg" width=48px height=48px/>  <img src="https://user-images.githubusercontent.com/65143821/143433804-723b67d0-54b9-45eb-b7b4-8fedb454bc4b.png" width=48px height=48px/>

## Version
- .NET Framework 4.5
- .NET 5.0 (Core)

## Operating System
- Windows, Linux & MacOS (for.NET Core)

## Nuget Packages

- AWSSDK.Core
- AWSSDK.SSO
- AWSSDK.SSOOIDC
- Costura Fody

## Additiomal Informaion
- More information on AWS SDK V3: https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SSOOIDC/NSSOOIDC.html
