# ELMAH Fallback ErrorLog

## What is it ?

It's just an composite implementation of the Elmah.ErrorLog abstract class to add fallback feature to ELMAH.

You can copy the source into your projects or install the [nuget package](http://nuget.org/packages/Elmah.FallbackErrorLog/).


## Configuration sample

### Typical legacy Elmah configuration

	<configuration>
	  <configSections>
	    <sectionGroup name="elmah">
	      <section name="security" requirePermission="false" type="Elmah.SecuritySectionHandler, Elmah" />
	      <section name="errorLog" requirePermission="false" type="Elmah.ErrorLogSectionHandler, Elmah" />
	      <section name="errorMail" requirePermission="false" type="Elmah.ErrorMailSectionHandler, Elmah" />
	      <section name="errorFilter" requirePermission="false" type="Elmah.ErrorFilterSectionHandler, Elmah" />
	    </sectionGroup>
	  </configSections>
	  <elmah>
		<errorLog type="Elmah.SqlErrorLog, Elmah" connectionStringName="DB_ELMAH"  applicationName="Blog" >
		<!-- OR...
		<errorLog type="Elmah.XmlFileErrorLog, Elmah" logPath="~/App_Data/Logs" />
		-->
	  </elmah>
	</configuration>

### New configuration with fallback feature :

	<configuration>
	  <configSections>
	    <sectionGroup name="elmah">
	      <section name="security" requirePermission="false" type="Elmah.SecuritySectionHandler, Elmah" />
	      <section name="errorLog" requirePermission="false" type="Elmah.FallbackErrorLogSectionHandler, Elmah.FallbackErrorLog" />
	      <section name="errorMail" requirePermission="false" type="Elmah.ErrorMailSectionHandler, Elmah" />
	      <section name="errorFilter" requirePermission="false" type="Elmah.ErrorFilterSectionHandler, Elmah" />
	    </sectionGroup>
	  </configSections>
	  <elmah>
	    <errorLog type="Elmah.FallbackErrorLog, Elmah.FallbackErrorLog" >
			<add type="Elmah.SqlErrorLog, Elmah" connectionStringName="DB_ELMAH"  applicationName="Blog" />
			<add type="Elmah.XmlFileErrorLog, Elmah" logPath="~/App_Data/Logs" />
			<add type="Elmah.MemoryErrorLog, Elmah" size="30" />
		</errorLog>
	  </elmah>
	</configuration>

## Limitations, caveats, known bugs

* Limit on the number of items that can be paginated (approx. 800k !) on the error log display page.

[Let me know](https://github.com/eric-b/Elmah.FallbackErrorLog/issues) if you have troubles with use of this library.


## See also

A very nice article about ELMAH : [ELMAH - Error Logging Modules And Handlers](http://dotnetslackers.com/articles/aspnet/ErrorLoggingModulesAndHandlers.aspx)

