# VirtualHosts and hosts file entries generator for XAMPP

## Setting everything up
Copy VirtualHosts.exe to XAMPP root folder. Create new folder "home", it's where your projects will reside.
The folder structure should be like this:
```
home/domain/public_html - for http://domain/ and http://www.domain/
home/domain/subdomain - for http://subdomain.domain/ and http://www.subdomain.domain/
```

Finally add these lines to the end of your httpd.conf file:

```
DocumentRoot "C:/xampp/home"
<Directory "C:/xampp/home">
    Options Indexes FollowSymLinks Includes ExecCGI
    AllowOverride All
    Require all granted
</Directory>
Include conf/vhosts.cnf
```

## Using
The application requires Administrative rights in order to write to %WINDIR%/System32/drivers/etc/hosts file.
To generate VirtualHosts file and add entries to hosts file just run the app. To remove entries from hosts file add "restore" or "clean" parameter. You can use shortcuts in the package.

## Customizing
You can edit included Template.txt file in you want to have custom entries for your Apache VirtualHosts configuration. "{0}" is for server name and "{1}" is path.