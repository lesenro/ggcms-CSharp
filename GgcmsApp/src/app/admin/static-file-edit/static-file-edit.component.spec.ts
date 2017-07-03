import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StaticFileEditComponent } from './static-file-edit.component';

describe('StaticFileEditComponent', () => {
  let component: StaticFileEditComponent;
  let fixture: ComponentFixture<StaticFileEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StaticFileEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StaticFileEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
