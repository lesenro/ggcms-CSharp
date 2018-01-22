import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModulesOptionsComponent } from './modules-options.component';

describe('ModulesOptionsComponent', () => {
  let component: ModulesOptionsComponent;
  let fixture: ComponentFixture<ModulesOptionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModulesOptionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModulesOptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
