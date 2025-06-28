import { useState } from "react";
import { type Result } from "@/lib/utils/result.util";

// Command function type
type CommandFn<TData, TVariables> = (
    variables: TVariables
) => Promise<Result<TData, Error>>;

// Command state
interface CommandState<TData> {
    data: TData | null;
    error: Error | null;
    isLoading: boolean;
    isSuccess: boolean;
    isError: boolean;
}

// Command options
interface CommandOptions<TData, TVariables> {
    onSuccess?: (data: TData, variables: TVariables) => void;
    onError?: (error: Error, variables: TVariables) => void;
    onSettled?: (data: TData | null, error: Error | null, variables: TVariables) => void;
}

// Command result
interface CommandResult<TData, TVariables> extends CommandState<TData> {
    mutate: (variables: TVariables) => Promise<TData | null>;
    reset: () => void;
}

/**
 * Custom hook for handling commands (mutations)
 */
export const useCommand = <TData, TVariables>(
    commandFn: CommandFn<TData, TVariables>,
    options?: CommandOptions<TData, TVariables>
): CommandResult<TData, TVariables> => {
    const [state, setState] = useState<CommandState<TData>>({
        data: null,
        error: null,
        isLoading: false,
        isSuccess: false,
        isError: false,
    });

    const reset = () => {
        setState({
            data: null,
            error: null,
            isLoading: false,
            isSuccess: false,
            isError: false,
        });
    };

    const mutate = async (variables: TVariables): Promise<TData | null> => {
        setState((prev) => ({
            ...prev,
            isLoading: true,
            error: null,
            isSuccess: false,
            isError: false,
        }));

        const result = await commandFn(variables);

        if (result.success) {
            setState({
                data: result.data,
                error: null,
                isLoading: false,
                isSuccess: true,
                isError: false,
            });

            options?.onSuccess?.(result.data, variables);
        } else {
            setState({
                data: null,
                error: result.error,
                isLoading: false,
                isSuccess: false,
                isError: true,
            });

            options?.onError?.(result.error, variables);
        }

        options?.onSettled?.(
            result.success ? result.data : null,
            result.success ? null : result.error,
            variables
        );

        return result.success ? result.data : null;
    };

    return {
        ...state,
        mutate,
        reset,
    };
};
