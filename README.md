# SpamBlock.Net
ASP.Net client for http:BL 

Block easily those spammers. Project Honey Pot is the first and only distributed system for identifying spammers and the spambots they use to scrape addresses from your website. 

Each time a visitor visits your website, SpamBlock checks if IP is blacklisted. In case visitor IP is blacklisted, it sends a 403.6 IP Rejected response. SpamBlock alows the request whenever it fails for any reason including lookup failures or an IPv6 address. SpamBlock works only with IPv4 addresses as Project Honey Pot only lists IPv4 addresses.

To use this library, you must register with [Project Honey Pot](https://www.projecthoneypot.org/) and you must also [request an Access Key](https://www.projecthoneypot.org/httpbl_configure.php) to make use of the service. 


This library has a IIS Module targeting .Net 4.6.1 and a ASP.NET Core middleware targeting .Net Standard 2.0


Download the latest [SpamBlock nuget package](https://www.nuget.org/packages/SpamBlock/).

1. Add access key to appSettings in web.config
```
<configuration>
  <appSettings>
    <add key="AccessKey" value="YOUR_ACCESS_KEY" />
	<add key="ThresholdThreatScore" value="50" /> <!-- Allows values with score less than threshold. Optional. Valid values 0 to 255. Default value is 50. -->
	<add key="MaxAgeInDays" value="10" />   <!-- Allows values with age less than max age. Optional. Valid values 0 to 255. Default value is 10. -->
  </appSettings>
</configuration>
```

2. Add the module in your web.config.

```
<configuration>
   <system.webServer>
      <modules>
         <add name="SpamBlockModule" type="SpamBlock.SpamBlockModule,SpamBlock"/>
      </modules>
   </system.webServer>
</configuration>
```

