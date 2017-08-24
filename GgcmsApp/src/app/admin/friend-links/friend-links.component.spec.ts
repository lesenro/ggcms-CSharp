import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendLinksComponent } from './friend-links.component';

describe('FriendLinksComponent', () => {
  let component: FriendLinksComponent;
  let fixture: ComponentFixture<FriendLinksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FriendLinksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FriendLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
