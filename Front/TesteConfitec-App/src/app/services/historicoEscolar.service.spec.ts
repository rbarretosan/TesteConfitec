/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { HistoricoEscolarService } from './historicoEscolar.service';

describe('Service: HistoricoEscolar', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HistoricoEscolarService]
    });
  });

  it('should ...', inject([HistoricoEscolarService], (service: HistoricoEscolarService) => {
    expect(service).toBeTruthy();
  }));
});
