import { Injectable } from '@angular/core';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PrintService {
  isPrinting = false;

  constructor(private router: Router) { }
  
  printDocument(documentName: string, Status: string) {
    // alert(documentName);
    // alert(Status);
    
    this.isPrinting = true;
    this.router.navigate(['/',
      { outlets: {
        'print': ['print', documentName]
      }}]);
  }

  onDataReady() {
    setTimeout(() => {
     // var z =document.getElementById('test');
      window.print();
      this.isPrinting = false;
      this.router.navigate([{ outlets: { print: null }}]);
    }, 3000);
  }
}
