import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './containers/home/home.component';
import { LoginComponent } from './containers/login/login.component';
import { LogoutComponent } from './containers/logout/logout.component';
import { PageNotFoundComponent } from './containers/page-not-found/page-not-found.component';
import { SignUpComponent } from './containers/sign-up/sign-up.component';
import { CallDetailComponent } from './containers/home/template/call/call-detail/call-detail.component';
import { ListDetailComponent } from './containers/home/template/call/list-detail/list-detail.component';
import { ContactDetailComponent } from './containers/home/template/contact/contact-detail/contact-detail.component';
import { ListContactComponent } from './containers/home/template/contact/list-contact/list-contact.component';
import { DefaultComponent } from './containers/home/template/default/default.component';
import { MessageDetailComponent } from './containers/home/template/message/message-detail/message-detail.component';
import { ListMessageComponent } from './containers/home/template/message/list-message/list-message.component';
import { NotificationDetailComponent } from './containers/home/template/notification/notification-detail/notification-detail.component';
import { ListNotificationComponent } from './containers/home/template/notification/list-notification/list-notification.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    LogoutComponent,
    PageNotFoundComponent,
    SignUpComponent,
    CallDetailComponent,
    ListDetailComponent,
    ContactDetailComponent,
    ListContactComponent,
    DefaultComponent,
    MessageDetailComponent,
    ListMessageComponent,
    NotificationDetailComponent,
    ListNotificationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
