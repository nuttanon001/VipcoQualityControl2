import { BaseRestService } from "./base-rest.service";
import { OnInit, Output, Input, EventEmitter } from "@angular/core";
import { FormGroup } from "@angular/forms";

export abstract class BaseInfoDialogComponent
  <Model, Service extends BaseRestService<Model>>
  implements OnInit{

  constructor(
    protected service: Service
  ) { }

  //Parameter
  @Input() InfoValue: Model;
  @Output() submitOrCancel: EventEmitter<Model | undefined> = new EventEmitter<Model | undefined>();
  InfoValueForm: FormGroup;
  step = 0;
  /*
   * Methods
   */
  setStep(index: number) {
    this.step = index;
  }
  nextStep() {
    this.step++;
  }
  prevStep() {
    this.step--;
  }

  ngOnInit(): void {
    if (this.InfoValue) {
      this.buildForm();
    }
  }
  // build form
  abstract buildForm(): void;
  // On submit
  onSubmit(): void {
    if (this.InfoValueForm) {
      if (this.InfoValueForm.valid) {
        this.InfoValue = this.InfoValueForm.value;
        this.submitOrCancel.emit(this.InfoValue);
      }
    }
  }
  // On cancel
  onCancel(): void {
    this.submitOrCancel.emit(undefined);
  }
}
