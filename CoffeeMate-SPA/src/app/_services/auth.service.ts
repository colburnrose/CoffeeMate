import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseURL = 'http://localhost:5000/api/account/';

constructor(private http: HttpClient) { }
// login method
login(model: any) {
  return this.http.post(this.baseURL + 'login', model).pipe(
    map((response: any) => {
      const user = response;
      if (user) {
        localStorage.setItem('token', user.token);
      }
    })
  );
}

}
