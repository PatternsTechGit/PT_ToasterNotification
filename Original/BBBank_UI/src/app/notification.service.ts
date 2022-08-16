import { Injectable } from '@angular/core';
   
import { ToastrService } from 'ngx-toastr';
   
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
   
  constructor(private toastr: ToastrService) { }
   title :'BBBank';
  showSuccess(message:any){
      this.toastr.success(message, this.title)
  }
   
  showError(message:any){
      this.toastr.error(message, this.title)
  }
   
  showInfo(message:any){
      this.toastr.info(message, this.title)
  }
   
  showWarning(message:any){
      this.toastr.warning(message, this.title)
  }
   
}