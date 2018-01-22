import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModulesColumnsComponent } from './modules-columns.component';

describe('ModulesColumnsComponent', () => {
  let component: ModulesColumnsComponent;
  let fixture: ComponentFixture<ModulesColumnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModulesColumnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModulesColumnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
