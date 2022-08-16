export interface ApiResponse {
    isError: boolean;
    message: string;
    responseException: ResponseException;
}
export interface ResponseException {
    exceptionMessage: string;
    validationErrors: ValidationError[]
}

export interface ValidationError {
    name: string;
    reason: string
}