import { Component, NgZone, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/core/service/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
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
      UserName: ["", Validators.required],
      Password: ["", Validators.required],
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.formData.invalid) {
      console.log(this.formData.value);
      return;
    }

    this.spinner.show();
    this.authService
      .login(this.formData.getRawValue())
      .pipe(
        finalize(() => this.spinner.hide())
      )
      .subscribe({
        next: (response: any) => {
          this.toastr.success("Đăng nhập thành công")
          this.navigate("/")
        },
        error: (error) => this.toastr.error("Login fail"),
      });
  }

  navigate(path: string): void {
    this.ngZone.run(() => this.router.navigateByUrl(path)).then();
  }
}
