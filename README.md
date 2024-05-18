# Config-Validator

[![NuGet Version](https://img.shields.io/nuget/v/ConfigValidator.Cli)](https://www.nuget.org/packages/ConfigValidator.Cli/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ConfigValidator.Cli)](https://www.nuget.org/packages/ConfigValidator.Cli/)

**Config-Validator** is an tool helping to validate configuration using automated pipelines and CLI.
<br>Configuration validation is setup using YAML with Validation Methods taken from [Definit.Validation](https://www.nuget.org/packages/Definit.Validation/) package.
<br>For now it supports Azure KeyVault and Azure AppConfiguration out of the box.

```yml
﻿GlobalTest.Value1:SharedSecret: IsConnectionString
GlobalTest.Value2: IsUrl
GlobalTest.Value3: IsUrl
```

Then it can be run in for example Azure Pipeline

```yml
pool:
  vmImage: ubuntu-latest

steps:
- bash: |
    docker pull hubiboar/config-validator:latest
    docker run -v $(Build.SourcesDirectory):/mnt hubiboar/config-validator:latest azure app-configuration-secret --file-path "../mnt/validation.yml" --key-vault-name "$(KV-Name)" --tenant-id $(TenantId) --client-id $(ClientId) --client-secret $(ClientSecret) --secret-name "AppConfigConnectionString"
  displayName: 'Docker Pull and Run'
```


