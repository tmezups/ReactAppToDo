type ResultStatus =
    | 'success'
    | 'error'
    | 'not found'
    | 'unauthorized'
    | 'forbid';

export interface IApiResult<T> {
    status: ResultStatus;
    data?: T;
}

export const apiPromise = <T>(
    status: ResultStatus,
    data?: T
): IApiResult<T> => ({
    status,
    data,
});