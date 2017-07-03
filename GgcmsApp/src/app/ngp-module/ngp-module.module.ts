import { ModuleWithProviders, NgModule } from '@angular/core';
import 'rxjs/Rx';
import {
  AccordionModule,
  AutoCompleteModule,
  CalendarModule,
  InputTextModule,
  ButtonModule,
  CheckboxModule,
  ChipsModule,
  DropdownModule,
  GrowlModule,
  ConfirmDialogModule,
  ConfirmationService,
  DialogModule,
  TreeTableModule, TreeModule, TreeNode, SharedModule,
  OverlayPanelModule,
  InputSwitchModule,
  MultiSelectModule,
} from 'primeng/primeng';

@NgModule({
  exports: [
    AccordionModule,
    AutoCompleteModule,
    CalendarModule,
    InputTextModule,
    ButtonModule,
    CheckboxModule,
    ChipsModule,
    DropdownModule,
    GrowlModule,
    ConfirmDialogModule,
    DialogModule,
    TreeTableModule, SharedModule, TreeModule,
    OverlayPanelModule,
    InputSwitchModule,
    MultiSelectModule,
  ],
  providers: [
    ConfirmationService,
  ]
})

export class NgpModule {
}


