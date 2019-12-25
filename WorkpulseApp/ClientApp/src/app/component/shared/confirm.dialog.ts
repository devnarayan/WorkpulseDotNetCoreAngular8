import { Component, OnInit, Inject } from '@angular/core';
import { NgModule } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'confirm-dialog',
  templateUrl: './confirm.dialog.html',
})

export class ConfirmDialog {
  title: string;

  constructor(
    public dialogRef: MatDialogRef<ConfirmDialog>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmModel) {
    this.title = data.title;
  }

}


interface ConfirmModel {
  title: string;
}
