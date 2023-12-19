import '@testing-library/jest-dom'
import {describe, expect, it} from 'vitest';
import ToDo from '../index';
import { render, screen, userEvent } from '../../../../utils/test-utils';
import { BrowserRouter } from 'react-router-dom';
import {fireEvent} from '@testing-library/react'



describe('ToDo logic tests', () => {


    
     it('should render the component', () => {
         render(<BrowserRouter><ToDo /></BrowserRouter>)
         expect(screen.getByText('Create Task')).toBeInTheDocument();
     });
     
    it('Should return posts when clicking fetch button', async () => {
        render(<BrowserRouter><ToDo /></BrowserRouter>)

        const todoTitle = screen.getByTestId(/todo-input/i);
        fireEvent.change(todoTitle, {target: {value: 'test'}})
        await userEvent.click(screen.getByTestId(/todo-button/i))

        expect(await screen.findByText('first post title')).toBeInTheDocument()

    })
});
