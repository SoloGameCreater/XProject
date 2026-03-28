#!/usr/bin/env python3
"""Analyze staged or unstaged git diff to decide commit message requirements."""

import argparse
import subprocess
import sys

CODE_EXTENSIONS = {".cs", ".py"}


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--cached", action="store_true", help="Analyze staged changes")
    args = parser.parse_args()

    cmd = ["git", "diff", "--numstat"]
    if args.cached:
        cmd.append("--cached")

    result = subprocess.run(cmd, capture_output=True, text=True)
    if result.returncode != 0:
        print(f"git diff failed: {result.stderr}", file=sys.stderr)
        sys.exit(1)

    total_files = 0
    total_added = 0
    total_deleted = 0
    code_files = 0
    code_added = 0
    code_deleted = 0

    for line in result.stdout.strip().splitlines():
        if not line:
            continue
        parts = line.split("\t", 2)
        if len(parts) < 3:
            continue
        added, deleted, path = parts
        # binary files show "-" for added/deleted
        a = int(added) if added != "-" else 0
        d = int(deleted) if deleted != "-" else 0
        total_files += 1
        total_added += a
        total_deleted += d

        if any(path.endswith(ext) for ext in CODE_EXTENSIONS):
            code_files += 1
            code_added += a
            code_deleted += d

    total_changed = total_added + total_deleted
    code_changed = code_added + code_deleted
    need_desc = code_files > 3 or code_changed > 10
    need_review = total_files > 10 or total_changed > 100

    print(f"total_files_changed={total_files}")
    print(f"total_added={total_added}")
    print(f"total_deleted={total_deleted}")
    print(f"total_changed_lines={total_changed}")
    print(f"code_files_changed={code_files}")
    print(f"code_added={code_added}")
    print(f"code_deleted={code_deleted}")
    print(f"code_changed_lines={code_changed}")
    print(f"need_description={need_desc}")
    print(f"need_review_prompt={need_review}")


if __name__ == "__main__":
    main()
