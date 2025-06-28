import * as React from "react";
import { type LucideIcon } from "lucide-react";
import type { VariantProps } from "class-variance-authority";

import { cn } from "@/lib/utils/cn.util";
import { Button } from "./button";

type InputProps = React.ComponentProps<"input"> & {
    action?: () => void;
    actionIcon?: React.ReactNode;
    actionPosition?: "left" | "right";
    actionVariant?: VariantProps<typeof Button>["variant"];
    className?: string;
    icon?: LucideIcon;
    iconPosition?: "left" | "right";
    type?: string;
};

function Input({
    action,
    actionIcon,
    actionPosition,
    actionVariant = "secondary",
    className,
    icon: Icon,
    iconPosition,
    type,
    ...props
}: InputProps) {
    const iconPositionClass = iconPosition === "left" ? "pl-3" : "pr-3";
    const actionPositionClass = actionPosition === "left" ? "pl-3" : "pr-1";

    return (
        <div
            className={cn(
                "flex border-input border file:border-0 selection:bg-primary dark:bg-input/30 bg-transparent file:bg-transparent rounded-md pl-3 py-1",
                "focus-visible:border-ring focus-visible:ring-ring/50 focus-visible:ring-[3px]",
                "aria-invalid:ring-destructive/20 dark:aria-invalid:ring-destructive/40 aria-invalid:border-destructive",
                className
            )}
        >
            {Icon && (
                <div className={cn("flex items-center", iconPositionClass)}>
                    <Icon className="w-4 h-4" />
                </div>
            )}
            <input
                type={type}
                data-slot="input"
                {...props}
                className="file:text-foreground placeholder:text-muted-foreground selection:text-primary-foreground flex h-8 w-full min-w-0 text-base shadow-xs transition-[color,box-shadow] outline-none file:inline-flex file:h-7 file:text-sm file:font-medium disabled:pointer-events-none disabled:cursor-not-allowed disabled:opacity-50 md:text-sm [&:-webkit-autofill]:!bg-transparent"
            />

            {actionIcon && action && (
                <div className={cn(" flex items-center", actionPositionClass)}>
                    <Button
                        onClick={action}
                        variant={actionVariant}
                        type="button"
                        size="icon"
                        className="w-8 h-8"
                    >
                        {actionIcon}
                    </Button>
                </div>
            )}
        </div>
    );
}

export { Input };
