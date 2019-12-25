import { TestBed } from '@angular/core/testing';

import { UserServiceService } from './user-service.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('UserServiceService', () => {
  let service = UserServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserServiceService,
        { provide: 'BASE_URL', value: 'https://localhost:44357' }]
    })
    service = TestBed.get(UserServiceService);    
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
