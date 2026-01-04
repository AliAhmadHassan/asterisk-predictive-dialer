# Asterisk Predictive Dialer (Project Silver)

Predictive dialing platform that monitors agent state and queue activity in
Asterisk AMI to place calls before operators become idle. This is a full
call-center stack built in C#/.NET: a dialer engine, Asterisk integration,
campaign and mailing management, Windows services, and a web dashboard.

## Executive summary
Project Silver is a production-grade predictive dialer with tight Asterisk AMI
integration. It automates SIP/queue configuration, manages campaigns and call
lists, and exposes operational dashboards (TMA/TME, operator metrics, queue
stats). The architecture is layered (DTO/BLL/DAL) and split into services,
desktop control tools, and a WebForms UI.

## What this project does
- Predictive dialing driven by live AMI events (queue member status, call
  states, queue stats) to keep operators productive.
- Campaign and mailing management with call status tracking and retries.
- Automatic generation of `sip.conf` and `queues.conf`, deployed to Asterisk
  via SFTP.
- Web dashboard with performance metrics (TMA/TME), operator stats, and
  campaign visibility.
- Recording and call log integration for operational oversight.
- Centralized logging for services and tools.

## How it works (end-to-end flow)
1. Operators, campaigns, and call lists are managed via the web UI and stored
   in MySQL.
2. The dialer engine consumes those records, evaluates agent availability, and
   issues AMI actions (originate, queue pause/unpause, queue status).
3. A dedicated AMI client library listens to Asterisk events and publishes
   them to services and dashboards.
4. Services update call statuses in the database and produce operational logs.
5. The dashboard surfaces queue and operator KPIs in near real time.

## Architecture
```
Web UI (ASP.NET WebForms) ---> BLL/DAL/DTO ---> MySQL
         |                          ^
         v                          |
WinForms Admin (RoboDiscagem) ------+
         |
         v
Dialer Services <--> AsteriskClient (AMI) <--> Asterisk
         |
         +--> SIP/Queue config generator --> FTP/File copy
```

## Key components (codebase tour)
- `Silver.AsteriskClient`: AMI client library that handles socket I/O, AMI
  login, event parsing, queue control, and originate commands.
- `Silver.RoboDiscagem`: WinForms control plane for campaign lifecycle,
  monitoring, and admin operations.
- `Silver.ProxyDiscagem` / `Silver.ProxyDiscagem.Server` /
  `Silver.ProxyDiscagem.Cliente`: Windows service and client components for
  dialer orchestration and remote control.
- `Silver.ServerEscuta` / `Silver.Servico.Escuta`: Windows services that listen
  to AMI events and persist them to storage.
- `Silver.UI.WebForm`: WebForms dashboard and management portal.
- `Silver.BLL` / `Silver.DAL` / `Silver.DTO` / `Silver.Common`: layered
  business logic, data access, contracts, and shared utilities.
- `AsteriskProxy`: standalone AMI proxy server/client/service for distributing
  AMI streams to other components.
- `Discador.Distribuicao`: MSI installer project for packaging the dialer.

## Telecom integration highlights
- AMI event ingestion for queue member status, pause/unpause, and call state.
- AMI actions for queue control and originate commands.
- Queue configuration generation with Asterisk routing strategies.
- Support for integrating with SIP agents and queue membership automation.

## Dialing strategies
The dialer supports Asterisk queue strategies such as `ringall`, `random`,
`linear`, `leastrecent`, `fewestcalls`, `rrmemory`, and `wrandom`.

## Technology stack
- C# / .NET Framework 4.0
- ASP.NET WebForms, WinForms, Windows Services
- MySQL (MySql.Data)
- Asterisk AMI (queue, originate, SIP, dialplan commands)
- FTP for config distribution
- AjaxControlToolkit, DotNet.Highcharts

## Quality and testing
- NUnit-based test project: `ProjetoTeste`
- Additional test harnesses: `Silver.Teste`, `Silver.Testes.Discador`
- Strong separation of concerns (DTO/BLL/DAL) for maintainability

## Why this stands out
This system combines telecom signaling, real-time event processing, and
multi-tier application design. It shows end-to-end ownership: building AMI
clients, orchestrating predictive dial flows, generating Asterisk configs,
packaging installers, and surfacing operational metrics to supervisors.
