import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QlikReportComponent } from './qlik-report.component';

describe('QlikReportComponent', () => {
  let component: QlikReportComponent;
  let fixture: ComponentFixture<QlikReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QlikReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QlikReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
