import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AlertComponent } from './alert.component';

@NgModule({
  imports: [CommonModule],
  declarations: [AlertComponent],
  exports: [AlertComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class AlertModule { }
