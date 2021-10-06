# RELEASE #

This RELEASE file is basic information for the changes made every version.

### x.x.x ###
* Added: Filter functionality for Dossiers in DeadLine Overview Page. WPJPBM-490

### 6.1.0 ###
```
Compatible with JPBM67
```
* Updated: Entity configuration based on field length changes for optienummer for optie_standaard and optie_gekozen tables and related views.

### 6.0.2 ###
* Added: Loader for the repair request details page while it is loading data from the endpoint. WPJPBM-441
* Added: Filter on the repair requests based on the icon that represents attention. WPJPBM-635
* Fixed: Printing a list PDF results in only one page in all modules. WPJPBM-647
* Fixed: Wrong buyer information is shown after selecting an object(add loaders and abort controller). WPJPBM-645
* Fixed: Chat message for a new quotation is shown in wrong chat. WPJPBM-646
* Fixed: Filter is resetted after zooming in on repair request from list. WPJPBM-612
* Fixed: Selecting another organization for rework results in an empty list. WPJPBM-614
* Fixed: Scrolling through list of subcontractors (betrokkenen = possible solvers) does not work. WPJPBM-631
* Added: Textual changes for tipToolText for tasks on a file. WPJPBM-652
* Fixed: Show repair request of type PreDelivery to buyers until the date of delivery. WPJPBM-414
* Fixed: Creating a work order based on validation conditions. WPJPBM-611 & WPJPBM-657
* Fixed: Exclamation mark not disappear in buyer login.
* Fixed: Company logo is not shown in email when sending a work order. WPJPBM-653
* Added: Possibility to finish/complete a repair request without work orders. WPJPBM-468

### 6.0.1 ###
* Added: Use of new column "voorkeurstijdstip_afspraak" (available since JPBM66 in melding table) when adding repair request and display on UI for Site Manager. WPJPBM-544 & WPJPBM-428
* Fixed: Adding date and time of email in "email_verzonden" field in contact table when sending out a workorder notification to a solver. WPJPBM-426
* Fixed: Sending email in case of "Forgot password" to work even when no visiting address defined to the organistion. WPJPBM-639
* Updated: UI related textual improvements. WPJPBM-640 & WPJPBM-642
* Fixed: Opening dossiers from menu showing the empty dossier directly where there are no dossiers. WPJPBM-649
* Fixed: Issues rights related issues that were in the system after WPJPBM-584. WPJBM-650

### 6.0.0 ###
```
BREAKING CHANGES: (Only when previous version is 5.x.x) Set values of  intern, intern_bestand_wijzigen, intern_relatie_zichtbaar, extern(if dossier is extern) to true in login_dossier_recht table.
```
* Fixed: Exclamation mark to appear immediately. WPJPBM-627
* Updated: Dossier Rights settings changed as per new fields. WPJPBM-584
* Added: Spectator Role in Huisinfo. WPJPBM-559

### 5.0.6 ###
* Added: Changes to move dossier files endpoint for new UI. Files can be moved from any section to any but not between general and building sections. Added condition so that only Buyer Guide Role can move files to and from archive. Added filter so buyer cannot view archived files. Sorted files by modified date. WPJPBM-587
* Updated: Improved Preview of Document for Mobile Screen in Document Preview Functionality. WPJPBM-606

### 5.0.5 ###
* HOTFIX: Download Files with iOS.(Force unregister old service worker and let browser install new service worker) WPJPBM-625

### 5.0.4 ###
* Fixed: Download Files with iOS. WPJPBM-625

### 5.0.3 ###
* Updated: Improved Dossiers UI thumbnail preview for adding existing Huisinfo files. WPJPBM-571

### 5.0.2 ###
* HOTFIX: Prevent buyers to see repair requests of type Inspection(also the ones which are not linked to any Opname).

### 5.0.1 ###
* Fixed: Clicking on an important message to open and highlight the message in the chat. WPJPBM-615
* Fixed: After switching the project, closing of selected chat and opened chat box from previous project. WPJPBM-617
* Fixed: News content HTML Images/Content to auto resize based on responsiveness of screen. WPJPBM-620

### 5.0.0 ###
```
Compatible with JPBM66
BREAKING CHANGES: Added new centralized database 'HUISINFO_CENTRAL'.
Add new connection string key "CentralDatabaseConnection" with value as connection string to HUISINFO_CENTRAL in appsettings.json file
```
* Added: Dossier Feature(Also changed the way login table was used, it is using some fields from the HUISINFO_CENTRAL db now). WPJPBM-472
* Added: A user now can have multiple roles in multiple projects(Roles editable from JPBM for now.)
* Fixed: Only open action items should be visible on Buyer's guide dashboard. WPJPBM-465

### 4.0.0 ###
```
BREAKING CHANGES: Windows service needs to be registered for daily email notifications. Please check more info below.
```
* New: App converted to Progressive Web App(PWA) with some extra settings in appsettings.json (AppName and AppShortName should be added with correct names for the client in appsettings.json)
* New: Windows service created for DailyEmailNotifications and removed existing Cron Job from the web app.
    ```
    Commands for windows service (Run it in command prompt as Administrator):
    * Create a service: sc create <Name of service> binpath= "<Path to executable>" \""<Escaped path to appsettings.json file>"\"" displayname= <Display name of service> start= delayed-auto
    Example:
        sc create HuisinfoDailyEmails binpath= "C:\inetpub\wwwroot\huisinfo_windows_service\4.0.0\Portal.JPDS.WorkerService.exe \""C:\\inetpub\\wwwroot\\dev_huisinfo_nl\\appsettings.json"\"" displayname= "Huisinfo DailyEmails" start= delayed-auto
    * Start a service: sc start <Name of service>
    * Stop a service: sc stop <Name of service>
    * Remove a service: sc delete <Name of service>
    ```

### 3.0.3 ###
* Fixed: Missing contact info(phone numbers and email addresss) in email with work order to oplosser. WPJPBM-443
* Added: Debug logs for daily email notifications. (To use this update Logging.LogLevel.Default valut to "Debug" in appsettings.json)

### 3.0.2 ###
* HOTFIX: Chat messages showing duplicates on UI every 10 seconds. Issue detected: DateTime modal binder behaves differently for Dot Net 5.0

### 3.0.1 ###
* Fixed: 48 hours reminder to not show for completed workorders.
* Fixed: Update Date with current Time in gecontroleerd_op field in oplosser table.
* Fixed: Email template for workorder to align content to top.
* Fixed: Email template for workorder to create line-breaks for multiline fields.
* Fixed: Email template for workorder to show correct value in field "Datum ingelicht" when sending reminder for the first time.
* Fixed: Disable product/service selector when repair request is marked as completed. Also for sub-product/service and sub-sub-product/service.
* Fixed: Oplosser's Grid to show column title as "Relatie" instead of "Employee".
* Fixed: Reset Password Issue when no Complaints Management Module configured.
* Fixed: To not show Buyer's Guide module when the login role is for Oplosser(onderaannemer).

### 3.0.0 ###
```
Compatible with JPBM65
BREAKING CHANGES: Upgraded to Dot Net 5.0. Need DotNet 5.0 hosting bundle installed on server for running the website.
Add new appsettings "ResolverOrSiteManagerNotificationTime" in appsettings.json file for daily notifications. 
Add new appsettings "SurveyReportPath" in appsettings.json file for survey report. 
```
* Added: Construction Site Manager & Oplosser's Module with Daily email notifications. WPJPBM-361
* Added: New appsetting "CorsAllowAll" to allow access from different applications for endpoints(Just for development purpose).
* Fixed: Tasks on buyers guide dashboard(Acties) to show only logged in Employee's tasks. WPJPBM-400
* Fixed: Standard options to be available on the day same as closing date. WPJPBM-367
* Updated: Using local Report from RDL file instead of SSRS.
* Updated: Improvement in daily email notifications.
* Updated: Direct WorkOrder link page to have Reject button. Also some UI Improvements.
* Custom: Hiding Drawer Menu in Nazorg app for Batenburg based on url 'nazorg.batenburgbv.nl'

### 2.0.9 ###
* Added: BCC to "jpdatabasesolutions@gmail.com" for all daily notifications emails.
* Added: Logs when daily email notification task fails.

### 2.0.8 ###
* Added: Possibility to delete standard options images in option configuration page. WPJPBM-186
* Fixed: Show warning above button "Verstuur" on add new repair request page. WPJPBM-338

### 2.0.7 ###

* Added: Log errors to general website logs when daily notification email fails. WPJPBM-328
* Added: System Chat Messages when a document was uploaded. WPJPBM-303
* Fixed: Email layout for repair request. WPJPBM-334
* Fixed: Small UI issues with creating repair request. WPJPBM-325
* Fixed: On Mobile when sending a message, the chats do not scroll to the bottom. WPJPBM-318

### 2.0.6 ###
* Added: Improved way to add images to multiple standard options. WPJPBM-323
* Fixed: Uploading images to standard options not working sometimes issue. WPJPBM-324

### 2.0.5 ###
* HOTFIX: Download files authentication issue because of session expire with Login as 'Remember Me'.
* Fixed: On mobile when going to Keuzelijst there is a back button. The back button will be hidden now unless there is some category selected.
* Fixed: Clicking on image in Keuzelijst to not toggle the bottom part visiblity from where we can add the quantity.
* Fixed: On mobile when typing a chat message with multiple lines then chatbox suddenly disappears.
* Fixed: Moving images around in the options list does not result in images being in a different order on the users portal. WPJPBM-292

### 2.0.4 ###
```
Compatible with Huisinfo Android v1.8
```
* New: Separate Endpoint "/api/survey/SendSecondSignatureEmail" created for SecondSignature Email.

### 2.0.3 ###
```
Compatible with Huisinfo Android v1.7
```
* New: Saving draft chat message text when navigating away from the textbox in Chat. WPJPBM-281
* Fixed: Endpoints for Delivery email, second signature email and small bugs and imporvements.

### 2.0.2 ###
* HOTFIX: Crashing of Android app due to incorrect version matching in the api with Google Play Store version 1.6
* Fixed: Recent top 5 saved messages not appearing because of NULL value in 'onderwerp' field in 'chat_bericht' table.
* Fixed: Count on the top is incorrect because of Chat created with buyer's guide and later on the person was removed as buyer's guide for the project. Fixed on front-end.

### 2.0.1 ###
* HOTFIX: Fix for the clients that are not using AfterCare module.
* Fixed: Unread messages not showing for new chats. WPJPBM-319

### 2.0.0 ###
```
Compatible with JPBM64
```
* New: Last 5 days counter based on the closing dates. WPJPBM-282
* New: Changes to new Layout. Recent 5 unread and recent 5 saved messages popover per project. WPJPBM-277
* New: Endpoints for Second Signature Process in Opname App.
* New: New user interface for buyer's guide.
* New: Buyer's guide is able to see Huisinfo as a buyer. WPJPBM-262
* New: Forgot password flow. WPJPBM-163, WPJPBM-17
* New: Change password after login.
* New: Integrate System Messages in the chat section. WPJPBM-272
* Fixed: Problems with Additional Descriptions in Shopping Cart. WPJPBM-295 & WPJPBM-296
* Fixed: Update status after digital signing to update date columns "datum_definitief" and "datum_vervallen" based on status. WPJPBM-240
* Fixed: Hide repair requests from inspections in the Service module (old MeldingenRegistratie) in Huisinfo. WPJPBM-260
* Fixed: Time-stamp of chat messages in Safari. WPJPBM-270
* Fixed: Translation fixes. WPJPBM-271, WPJPBM-272, WPJPBM-273
* Fixed: Switching between chats results in a white screen(Issue with browser translation tool). WPJPBM-274
* Fixed: Login only allowed when account is active. WPJPBM-266
* Updated: Checkbox hidden temporarily for checking Terms and Conditions(until the link is ready) on Quoations. WPJPBM-255
* Updated: Better UI of chat module.
* Updated: Redesign ordering process. WPJPBM-182
* Updated: Better way to show refresh the page when there is a new update of Huisinfo.

### 1.0.14 ###

* HOTFIX: Fixed to mark messages read(Front-end bug, sometimes failing due to different browser sizing and scrolling behaviour at client).

### 1.0.13 ###

* HOTFIX: Fixed the bug for planning when there is start-time in the view from database.

### 1.0.12 ###

* HOTFIX: Fixed to mark messages read(sometimes failing due to front-end bug).

### 1.0.11 ###

* HOTFIX: Fixed individual options to be displayed in Shopping Cart.

### 1.0.10 ###

* HOTFIX: Fixed to prevent adding duplicate chats when starting a new chat.

### 1.0.9 ###

* HOTFIX: Fixed to not show the repair requests in Nazorg module, that are linked to Inspections. WPJPBM-260

### 1.0.8 ###

* HOTFIX: Fixed to send PvO email to organisation when the buyer is an organisation.
* HOTFIX: Fixed to add organisation name in the in buyer1name when the buyer is organisation when adding an inspection.
* HOTFIX: Allow opname app access when no objects linked but the login account type is Employee.

### 1.0.7 ###

* Small code improvements.
* Fixed: Standard options to work even when Unit(eenheid) not defined.
* ADDED: Extra email addresses for Delivery Report. WPJPBM-244
* ADDED: WorkOrder Status update page. WPJPBM-243
* ADDED: ReportGenration endpoint using SSRS for Opname App.
* ADDED: Endpoints for Delivery(Opname) App.
* ADDED: Possiblity to switch between apps as new integration of MeldingenRegistratie and for new apps in future.
* ADDED: MeldingenRegistratie(Nazorg-AfterCare) module integration. WPJPBM-235
* Fixed to show standard options even when no Unit(Eenheid) is given.

### 1.0.6 ###

* Fixed numbers in front of menu items under "Woonwensen" to have 1a and 1b instead of two items with same number 1. Also fixed the spacing to align the rest of the menu items with numbers

### 1.0.5 ###

* HOTFIX: Issue-"Chat Messages showing unread even after read sometimes" Fix-"Preventing updating the chat message read datetime to earlier than the one stored in the database for chat participant"

### 1.0.4 ###

* HOT FEATURE: Show notification of new version of the app with a click to reload link. NO JIRA TICKET(Also this will reflect when there is an update after this version)
* Added feature for buyer to see the requested options. WPJPBM-238
* Fixed button on ThankYou page to point to the new requested options page. WPJPBM-228
* Fixed to prevent confirmation emails to be sent to clients. WPJPBM-234
* Fixed text on "Mijn offertes" page. WPJPBM-227

### 1.0.3 ###
```
Compatible with JPBM63 with database patch *20200409 - view_portal_email_notification.sql*
```
* HOTFIX: Fixed label "[geachte_informeel]" to be replaced in Email body template for Daily Notification Emails with correct value. WPJPBM-232

### 1.0.2 ###
* Fixed sender name on email. HOTFIX

### 1.0.1 ###

```
Compatible with JPBM63
```
* Added feature Request Individual Option. WPJPBM-53
* Fixed display of images of standard option. WPJPBM-222
* Fixed order of the standard options based on option number also added option number to be displayed on UI. WPJPBM-215
* Fixed to show news from specified date. WPJPBM-211
* Change the description "Winkelwagen" to "Mijn voorlopige opties". WPJPBM-218
* Fixed issues for properly displaying new line character on html. WPJPBM-217 & WPJPBM-220
* Fixed wrong value for the columns "ingevoerd_door" and "gewijzigd_door". WPJPBM-202
* Added new label "[geachte_informeel]" to be replaced in Email body template for Daily Notification Emails. WPJPBM-209
* Fixed to not show text 'per' on standard options when no unit value. Eg with 'per': "€ 325,00 per pst." WPJPBM-221
* Fixed email send issue to use default credentials when password not provided in datbase.

### 1.0.0 ###

* This version has its own API connecting to database, so not using Indicium API
* Basic new version of Portal application
* Version 1.0.0