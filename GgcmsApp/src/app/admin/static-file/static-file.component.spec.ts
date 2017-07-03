import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StaticFileComponent } from './static-file.component';

describe('StaticFileComponent', () => {
  let component: StaticFileComponent;
  let fixture: ComponentFixture<StaticFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StaticFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StaticFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
