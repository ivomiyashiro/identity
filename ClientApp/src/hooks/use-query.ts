import { useState, useEffect, useCallback, useRef } from "react";
import { type Result } from "@/lib/utils/result.util";

// Query function type
type QueryFn<TData> = () => Promise<Result<TData, Error>>;

// Query state
interface QueryState<TData> {
    data: TData | null;
    error: Error | null;
    isLoading: boolean;
    isSuccess: boolean;
    isError: boolean;
    isFetching: boolean;
}

// Query options
interface QueryOptions<TData> {
    enabled?: boolean;
    refetchOnMount?: boolean;
    refetchOnWindowFocus?: boolean;
    retry?: number;
    retryDelay?: number;
    onSuccess?: (data: TData) => void;
    onError?: (error: Error) => void;
    onSettled?: (data: TData | null, error: Error | null) => void;
}

// Query result
interface QueryResult<TData> extends QueryState<TData> {
    refetch: () => Promise<Result<TData, Error>>;
}

/**
 * Custom hook for handling queries
 */
export const useQuery = <TData>(
    queryFn: QueryFn<TData>,
    options?: QueryOptions<TData>
): QueryResult<TData> => {
    const {
        enabled = true,
        refetchOnMount = true,
        refetchOnWindowFocus = false,
        retry = 0,
        retryDelay = 1000,
        onSuccess,
        onError,
        onSettled,
    } = options || {};

    const [state, setState] = useState<QueryState<TData>>({
        data: null,
        error: null,
        isLoading: false,
        isSuccess: false,
        isError: false,
        isFetching: false,
    });

    const retryCountRef = useRef(0);
    const mountedRef = useRef(true);

    const executeQuery = useCallback(
        async (isRefetch = false): Promise<Result<TData, Error>> => {
            if (!enabled) {
                return {
                    IsSuccess: false,
                    IsFailure: true,
                    Error: new Error("Query disabled"),
                } as Result<TData, Error>;
            }

            setState((prev) => ({
                ...prev,
                isLoading: !isRefetch && prev.data === null,
                isFetching: true,
                error: null,
                isSuccess: false,
                isError: false,
            }));

            try {
                const result = await queryFn();

                if (!mountedRef.current) {
                    return result;
                }

                if (result.IsSuccess) {
                    setState({
                        data: result.Value,
                        error: null,
                        isLoading: false,
                        isSuccess: true,
                        isError: false,
                        isFetching: false,
                    });

                    retryCountRef.current = 0;
                    onSuccess?.(result.Value);
                } else {
                    if (retryCountRef.current < retry) {
                        retryCountRef.current++;

                        setTimeout(() => {
                            if (mountedRef.current) {
                                executeQuery(isRefetch);
                            }
                        }, retryDelay);

                        return result;
                    }

                    setState({
                        data: null,
                        error: result.Error,
                        isLoading: false,
                        isSuccess: false,
                        isError: true,
                        isFetching: false,
                    });

                    retryCountRef.current = 0;
                    onError?.(result.Error);
                }

                onSettled?.(
                    result.IsSuccess ? result.Value : null,
                    result.IsSuccess ? null : result.Error
                );

                return result;
            } catch (error) {
                const errorObj =
                    error instanceof Error ? error : new Error(String(error));

                if (!mountedRef.current) {
                    return {
                        IsSuccess: false,
                        IsFailure: true,
                        Error: errorObj,
                    } as Result<TData, Error>;
                }

                if (retryCountRef.current < retry) {
                    retryCountRef.current++;

                    setTimeout(() => {
                        if (mountedRef.current) {
                            executeQuery(isRefetch);
                        }
                    }, retryDelay);

                    return {
                        IsSuccess: false,
                        IsFailure: true,
                        Error: errorObj,
                    } as Result<TData, Error>;
                }

                setState({
                    data: null,
                    error: errorObj,
                    isLoading: false,
                    isSuccess: false,
                    isError: true,
                    isFetching: false,
                });

                retryCountRef.current = 0;
                onError?.(errorObj);
                onSettled?.(null, errorObj);

                return { IsSuccess: false, IsFailure: true, Error: errorObj } as Result<
                    TData,
                    Error
                >;
            }
        },
        [queryFn, enabled, retry, retryDelay, onSuccess, onError, onSettled]
    );

    const refetch = useCallback(async (): Promise<Result<TData, Error>> => {
        retryCountRef.current = 0;
        return executeQuery(true);
    }, [executeQuery]);

    // Initial fetch on mount
    useEffect(() => {
        if (enabled && refetchOnMount) {
            executeQuery();
        }
    }, [enabled, refetchOnMount, executeQuery]);

    // Refetch on window focus
    useEffect(() => {
        if (!refetchOnWindowFocus || !enabled) {
            return;
        }

        const handleFocus = () => {
            if (mountedRef.current) {
                executeQuery(true);
            }
        };

        window.addEventListener("focus", handleFocus);
        return () => window.removeEventListener("focus", handleFocus);
    }, [refetchOnWindowFocus, enabled, executeQuery]);

    // Cleanup on unmount
    useEffect(() => {
        return () => {
            mountedRef.current = false;
        };
    }, []);

    return {
        ...state,
        refetch,
    };
};
