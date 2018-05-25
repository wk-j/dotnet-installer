## Create Installer using Wix

- [x] .NET Core Console
- [x] .NET Core Web API

## Ubuntu

```bash
sudo dpkg -i UbntuWeb.0.1.0.deb
sudo systemctl enable UbuntuWeb.service
sudo systemctl start UbuntuWeb

journalctl --unit UbuntuWeb --follow
systemctl status UbuntuWeb
```