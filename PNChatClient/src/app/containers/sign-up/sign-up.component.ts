import { Component, NgZone, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';

import { AuthenticationService } from 'src/app/core/service/authentication.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {
  formData!: FormGroup;
  submitted: boolean = false;

  constructor(
    private authService: AuthenticationService,
    private formBuilder: FormBuilder,
    private ngZone: NgZone,
    private router: Router,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.formData = this.formBuilder.group({
      FullName: ["", Validators.required],
      UserName: ["", Validators.required],
      Email: ["", Validators.required],
      Phone: ["", Validators.required],
      Password: ["", Validators.required],
      CheckTerm: [false, Validators.required],
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.formData.invalid || !this.formData.get('CheckTerm')?.value) {
      return;
    }
    console.log('Checked');
    this.spinner.show();
    this.authService
      .signUp(this.formData.getRawValue())
      .pipe(
        finalize(() => this.spinner.hide()),
      )
      .subscribe({
        next: (response: any) => {
          this.toastr.success("Đăng ký tài khoản thành công")
          console.log(response);
          this.navigate("/dang-nhap")
        },
        error: (error) => this.toastr.error("Đăng ký thất bại"),
      });
  }

  navigate(path: string): void {
    this.ngZone.run(() => this.router.navigateByUrl(path)).then();
  }
}
