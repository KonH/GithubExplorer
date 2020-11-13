# Github Explorer

[![Build status](https://ci.appveyor.com/api/projects/status/kpkcc3urwdbv0819?svg=true)](https://ci.appveyor.com/project/KonH/githubexplorer)
[![codecov](https://codecov.io/gh/KonH/GithubExplorer/branch/master/graph/badge.svg)](https://codecov.io/gh/KonH/GithubExplorer)
![Nuget](https://img.shields.io/nuget/v/GithubExplorer.CommandLine)

## Description

Simple terminal wrapper to retrieve small information subset using Github API and serialize it.
This tool can be installed as .NET Core Global Tool, manual build is not required.

## Installation

```
dotnet tool install --global GithubExplorer.CommandLine
```

## Usage

### Repositories

```
github_explorer --target repositories --username %USER% --output repositories.json
```
```
info: GithubExplorer.Explorer[0]
      Retrieving user information for '...'
info: GithubExplorer.Explorer[0]
      Retrieving repositories for '...'
info: GithubExplorer.Explorer[0]
      Found ... repositories for '...'
info: GithubExplorer.Writer[0]
      Result saved into 'repositories.json'
```

=> repositories.json:
```

  {
    "Url": "https://api.github.com/repos/...",
    "HtmlUrl": "https://github.com/...",
    "CloneUrl": "https://github.com/...",
    "GitUrl": "git://github.com/...",
    "SshUrl": "git@github.com:...",
    "SvnUrl": "https://github.com/...",
    "MirrorUrl": null,
    "Id": ...,
    "NodeId": "MDEwOlJlcG9zaXRvcnkxNDI3NjY0NjY=",
    "Owner": {
        ...
    },
    "Name": "...",
    ...
  },
```