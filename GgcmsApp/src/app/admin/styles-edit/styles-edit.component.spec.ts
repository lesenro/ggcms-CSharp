import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StylesEditComponent } from './styles-edit.component';

describe('StylesEditComponent', () => {
  let component: StylesEditComponent;
  let fixture: ComponentFixture<StylesEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StylesEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StylesEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
