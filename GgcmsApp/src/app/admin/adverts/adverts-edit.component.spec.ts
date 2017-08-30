import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertsEditComponent } from './adverts-edit.component';

describe('AdvertsEditComponent', () => {
  let component: AdvertsEditComponent;
  let fixture: ComponentFixture<AdvertsEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvertsEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
