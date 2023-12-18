import createFetchMock from 'vitest-fetch-mock';
import { vi } from 'vitest';
import '@testing-library/jest-dom'
const fetchMocker = createFetchMock(vi);
fetchMocker.enableMocks();
import {describe, expect, it} from 'vitest';
import ToDo from '../index';
import { render, screen } from '../../../../utils/test-utils';
import { BrowserRouter } from 'react-router-dom';


describe('ToDo logic tests', () => {
   
    it('should render the component', () => {
        fetchMock.mockResponseOnce(JSON.stringify({ body: '' }) );
        render(<BrowserRouter><ToDo /></BrowserRouter>);
        expect(screen.getByText('Create Task')).toBeInTheDocument(); 
        console.log('fet', fetchMock.requests()[0].url)  
    });
});
