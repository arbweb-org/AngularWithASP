import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { GridModule } from '@progress/kendo-angular-grid';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { ButtonsModule } from "@progress/kendo-angular-buttons";
import { IconsModule } from "@progress/kendo-angular-icons";
import { LabelModule } from "@progress/kendo-angular-label";
import { InputsModule } from "@progress/kendo-angular-inputs";
import { LayoutModule } from "@progress/kendo-angular-layout";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports:
    [
      BrowserModule, HttpClientModule,
      AppRoutingModule, GridModule,
      FormsModule,
      ReactiveFormsModule,
      InputsModule,
      LabelModule,
      ButtonsModule,
      IconsModule,
      LayoutModule
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
